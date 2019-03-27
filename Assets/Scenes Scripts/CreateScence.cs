using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreateScence : MonoBehaviour
{
    public Collider Bound;
    public Text bang;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other){
        
        if(other.tag == "Player"){
            
            Create();
        }
    }

    // Update is called once per frame
    void Create()
    {
        float length = Bound.bounds.extents.z * 4;
        Vector3 addtion = new Vector3(0,0,length);
        Vector3 newPos = transform.position + addtion;
        Instantiate(gameObject, newPos, transform.rotation);
        StartCoroutine(Die());
    }

    IEnumerator Die(){
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }  
}
