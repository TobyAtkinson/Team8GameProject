using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
  void LateUpdate()
    {
        Vector3 NewPos = player.position;
        NewPos.y = player.position.y;
        transform.position = NewPos;
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
