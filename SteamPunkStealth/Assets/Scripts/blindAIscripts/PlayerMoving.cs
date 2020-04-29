using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
   
    AudioManager manager;
    PlayerMovement movement;
    bool finished = true;

    bool isMoving;
    bool playingSound;

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
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && movement.isGrounded)
        {
            //manager.Play("Run");
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if(isMoving == true && playingSound == false)
        {
            manager.Play("Run");
            playingSound = true;
        }
        else if(isMoving == false && playingSound == true)
        {
            manager.StopPlaying("Run");
            playingSound = false;
        }


        /*
         if(moving() && finished)
         {
             playRun();
             finished = false;
         }


         else
         {
             if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
             {
                 manager.StopPlaying("Run");
             }
         }
         */

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
