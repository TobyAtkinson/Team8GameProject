using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockGadgetReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Shock() 
    {
        Debug.Log("Enemy Shocked");
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine("ShockDone", 0f);
    }

    IEnumerator ShockDone(float TimeShocked) 
    {
        yield return new WaitForSeconds(10);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
