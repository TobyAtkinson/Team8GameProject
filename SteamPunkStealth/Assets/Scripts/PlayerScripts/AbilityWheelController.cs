using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWheelController : MonoBehaviour
{
    public int AbilityChoice;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MouseDown()
    {
        if (gameObject.name == "Goggle Selection")
        {
            AbilityChoice = 1;
        }

        if (gameObject.name == "Teleport Selection")
        {
            AbilityChoice = 2;
        }

        if (gameObject.name == "Shock Selection")
        {
            AbilityChoice = 3;
        }
    }

    public void MouseEnter()
    {
            Debug.Log("Goggles Highlighted");
            gameObject.transform.localScale = gameObject.transform.localScale * 1.1f;
    }

    public void MouseExit()
    {
        if (gameObject.name == "Goggle Selection")
        {
            gameObject.transform.localScale = gameObject.transform.localScale / 1.1f;
        }

        if (gameObject.name == "Teleport Selection")
        {
            gameObject.transform.localScale = gameObject.transform.localScale / 1.1f;
        }

        if (gameObject.name == "Shock Selection")
        {
            gameObject.transform.localScale = gameObject.transform.localScale / 1.1f;
        }
    }
}
