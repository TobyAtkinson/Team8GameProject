using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        Destroy(this.gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter() 
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

}
