namespace PedometerU.Tests
{

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;

    public class StepCounter : MonoBehaviour
    {

        private enum ControlMode
        {
            Tank,
            Direct,
            Auto
        }

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;
        [SerializeField] private Animator m_animator;
        [SerializeField] private Rigidbody m_rigidBody;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;
        private readonly float m_backwardsWalkScale = 0.16f;
        private readonly float m_backwardRunScale = 0.66f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;

        private bool m_isGrounded;
        private List<Collider> m_collisions = new List<Collider>();

        //add auto function
        private Pedometer pedometer;
        private bool m_isMoving = false;
        public int previous_steps = 0; // change back to private
        public int current_steps = 0; // change back to private
        public double previous_distance;
        public double current_distance;

        public Text stepText;
        public Text distanceText;
        public Text speedText;
        public Text resultText;
        public Text gemText;
        public CanvasGroup panelMessage;

        public AudioSource monsterGrowl;
        public AudioSource monsterFootsteps;
        public AudioSource story;
        public AudioSource gemSound;
        private int Max_warnTime = 5;
        private int warningTime = 0;
        private bool IsAlive = true;

        public CameraShake cameraShake;

        void Start()
        {
            // Create a new pedometer
            pedometer = new Pedometer(OnStep);

            // Reset UI
            previous_distance = 0.0f;
            current_distance = 0.0f;
            resultText.text = "";
            OnStep(0, 0);

            GameController.controller.Reset();

            InvokeRepeating("UpdateMoving", 0.0f, 0.3f);
            InvokeRepeating("UpdateSpeed", 0.0f, 10f);
            InvokeRepeating("Warning", 10.0f, 17.0f);
        }

        void UpdateMoving()
        {
            m_isMoving = current_steps != previous_steps;
        }

        void UpdateSpeed()
        {
            // Convert metres to km then 10sec to an hour
            speedText.text = ((current_distance - previous_distance) * 0.001 * 360).ToString("F2") + " km/h";
            previous_distance = current_distance;
        }

        void Warning()
        {
            // When player is not moving, warn for monster attacks
            if (!m_isMoving)
            {
                warningTime++;

                if (warningTime < Max_warnTime)
                {
                    monsterGrowl.Play();
                    Invoke("PlayFootSteps", monsterGrowl.clip.length);

                    switch (warningTime)
                    {
                        case 1:
                            story.Pause();
                            resultText.text = "Monster is catching up, \nRUN!";
                            panelMessage.alpha = 1;
                            break;
                        case 3:
                            GameController.controller.gems = (int)(GameController.controller.gems * 0.8);
                            gemText.text = GameController.controller.gems.ToString();
                            resultText.text = "Monster robbed you of 20% gems!";
                            break;
                        case 4:
                            resultText.text = "Run or die!";
                            break;
                    }
                }
                else
                {
                    // Maximum warnings result in player's end
                    // Display text and go to Results Screen
                    monsterGrowl.Play();
                    resultText.text = "K.O.";
                    GameController.controller.lose = true;
                    Invoke("EndRun", 5f);
                }
            }

            // Once player starts moving from a previous stop,
            // Resume story and reset warning 
            else if (!story.isPlaying)
            {
                story.Play();
                warningTime = 0;
                panelMessage.GetComponent<FadeScript>().Fade();
            }
        }

        private void PlayFootSteps()
        {
            Handheld.Vibrate();
            monsterFootsteps.Play();
            StartCoroutine(cameraShake.Shake(.9f, .015f, 9));
        }

        //IEnumerator Wait()
        //{
        //    yield return new WaitForSeconds(4);
        //}

        public void EndRun()
        {
            GameController.controller.duration = Time.timeSinceLevelLoad / (60.0f);
            GameController.controller.speed = (float)GameController.controller.distance / (GameController.controller.duration / (60.0f));

            SceneManager.LoadScene("Results");
        }

        private void OnStep(int steps, double distance)
        {
            GameController.controller.distance = distance * 3.28084 * 0.0003048;
            previous_steps = current_steps;
            current_steps = steps;
            current_distance = distance;

            stepText.text = steps.ToString();
            distanceText.text = (distance * 0.001).ToString("F2") + " km";
        }

        private void OnDisable()
        {
            // Release the pedometer
            pedometer.Dispose();
            pedometer = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    if (!m_collisions.Contains(collision.collider))
                    {
                        m_collisions.Add(collision.collider);
                    }
                    m_isGrounded = true;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true; break;
                }
            }

            if (validSurfaceNormal)
            {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
            else
            {
                if (m_collisions.Contains(collision.collider))
                {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) { m_isGrounded = false; }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }

        void Update()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                case ControlMode.Auto:
                    AutoUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_wasGrounded = m_isGrounded;
        }


        private void TankUpdate()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            bool walk = Input.GetKey(KeyCode.LeftShift);

            if (v < 0)
            {
                if (walk) { v *= m_backwardsWalkScale; }
                else { v *= m_backwardRunScale; }
            }
            else if (walk)
            {
                v *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
            transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

            m_animator.SetFloat("MoveSpeed", m_currentV);

            // When user press up arrow key
            // Create steps and distance
            if (v > 0)
            {
                double d = current_distance + 80;
                int s = current_steps + 1;
                GameController.controller.distance = d * 3.28084 * 0.0003048;
                previous_steps = current_steps;
                current_steps = s;
                current_distance = d;

                stepText.text = s.ToString();
                distanceText.text = (d * 3.28084 * 0.0003048).ToString("F2") + " km";
            }

            //JumpingAndLanding();
        }

        private void DirectUpdate()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Transform camera = Camera.main.transform;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding();
        }


        private void AutoUpdate()
        {

            if (!m_isMoving)
            {
                m_currentV = 0;
                m_animator.SetFloat("MoveSpeed", m_currentV);
            }
            else
            {
                Vector3 direction = new Vector3(0, 0, 1);

                m_currentV = 1.0f;
                m_moveSpeed = 2.0f;
                transform.Translate(direction * m_moveSpeed * Time.deltaTime);
                m_animator.SetFloat("MoveSpeed", m_currentV);
            }

            //JumpingAndLanding();
        }

        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
            {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }

            if (!m_wasGrounded && m_isGrounded)
            {
                m_animator.SetTrigger("Land");
            }

            if (!m_isGrounded && m_wasGrounded)
            {
                m_animator.SetTrigger("Jump");
            }
        }
    }

}
