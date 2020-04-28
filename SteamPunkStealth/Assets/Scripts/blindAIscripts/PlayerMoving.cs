using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
   
    AudioManager manager;
    PlayerMovement movement;
    bool finished = true;
    // Start is called before the first frame update
    void Start()
    {
        GameObject AudioManager = GameObject.Find("Audio Manager");
        manager = AudioManager.GetComponent<AudioManager>();

        GameObject Player = GameObject.Find("NewPlayer");
        movement = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && movement.isGrounded)
        {
            manager.Play("Run");

          

        }
       
       /*
        if(moving() && finished)
        {
            playRun();
            finished = false;
        }
     
    */
        else
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            {
                manager.StopPlaying("Run");
            }
        }
       
    }

    bool moving()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            return true;
        return false;
    }

    void playRun()
    {
        manager.PlayOnce("Run");
    }

   
}
