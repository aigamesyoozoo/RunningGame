using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndRun : MonoBehaviour
{
    // Start is called before the first frame update
    public void End(){
        SceneManager.LoadScene("Results");
    }

}
