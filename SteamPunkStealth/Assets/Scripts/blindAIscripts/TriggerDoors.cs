using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoors : MonoBehaviour
{
    GameObject door1;
    GameObject door2;
    // Start is called before the first frame update
    void Start()
    {
        door1 = GameObject.Find("gate1");
        door2 = GameObject.Find("gate2");
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            door1.transform.Rotate(0, 40, 0);
            door2.transform.Rotate(0, -55, 0);
            Destroy(this);
        }
    }
}
