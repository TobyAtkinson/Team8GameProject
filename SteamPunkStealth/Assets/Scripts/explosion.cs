using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{

    public GameObject hitbox;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(hitbox, 0.5f);
        Destroy(this.gameObject, 5);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
