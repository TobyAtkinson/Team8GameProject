using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedEffect : MonoBehaviour
{
    private Vector3 target;

    private void Awake()
    {
        target = Camera.main.transform.position;
    }
    void LateUpdate()
    { 
        target.y = transform.position.y;
        transform.LookAt(target);
    }
}
