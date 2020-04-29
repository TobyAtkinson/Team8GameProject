using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoors : MonoBehaviour
{
    public GameObject gateGroupOpen;
    public GameObject gateGroupClosed;
    public GameObject blindGuard;
    public GameObject waypoint;
    public GameObject objective1;
    public GameObject objective2;
    public GameObject healthBargroup;
        


   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            blindGuard.SetActive(true);
            gateGroupOpen.SetActive(false);
            gateGroupClosed.SetActive(true);
            waypoint.SetActive(false);
            objective1.SetActive(false);
            objective2.SetActive(true);
            healthBargroup.SetActive(true);
            Destroy(this);
            
        }
    }
}
