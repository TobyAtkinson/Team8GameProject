using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
   
    AudioManager manager;
    PlayerMovement movement;
    bool finished = true;

    bool jumped = false;
    bool isMoving;
    bool playingSound;
    bool playingSprint;
    bool isSprinting;
    bool notGrounded;
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
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && movement.isGrounded && !movement.isCrouched)
        {
            //manager.Play("Run");
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if(isMoving == true && playingSound == false && !isSprinting )
        {
            manager.Play("Run");
            playingSound = true;
        }
        else if(isMoving == false && playingSound == true )
        {
            manager.StopPlaying("Run");
            playingSound = false;
        }

        else if(isMoving == true && isSprinting)
        {
            manager.StopPlaying("Run");
            playingSound = false;
        }

       if(Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }

       else
        {
            isSprinting = false;
        }

        if (isSprinting && playingSprint == false )
        {
            manager.Play("Sprint");
            manager.Play("SprintingFootsteps");
            playingSprint = true;
          
            

        }
        else
             if (!isSprinting && playingSprint )
        {
            manager.StopPlaying("Sprint");
            manager.StopPlaying("SprintingFootsteps");
            playingSprint = false;
           

        }
     
    }

    bool moving()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            return true;
        return false;
    }


   void isNotGrounded()
    {
        if (!movement.isGrounded)
        {
            jumped = true;
        }
       
      
    }

    void afterJump()
    {
        isNotGrounded();
        if(jumped && movement.isGrounded)
        {
            manager.Play("afterJumpImpact");
            if (manager.hasFinishedPlaying("afterJumpImpact"))
                jumped = false;
        }
    }
    void playRun()
    {
        manager.PlayOnce("Run");
    }

    bool isSoundCurrentlyPlaying()
    {
        if (playingSound || playingSprint)
            return true;
        return false;
    }

   
}
