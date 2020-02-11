using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoggleController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {

    }


    void OnCollisionEnter(Collision Col) 
    {
        if (Col.gameObject.tag == "Enemy") 
        {

        }
    }
}
