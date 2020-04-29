using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    float x;
    float z;
    Rigidbody playerRB;
    public CharacterController Controller;
    Vector3 inputVector;

    public Slider staminaBar;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public bool isSprinting;
    public bool canSprint = true;
    public float SprintCountdown;
    public float staminaMax;
    public float SprintCooldownCount;
    public float wallRunLength = 10f;
    public bool isWallRunningUp;
    public bool isWallRunningSide;

    AudioManager manager;

    // New bool added by toby, registers if the player is currently locked in an animation like an execution
    public bool movementLocked;
    
    #region Speeds
    public float speed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    #endregion 
    Vector3 move;
    public AnimationCurve WallRunArc;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public GameObject playerCam;
    Vector3 velocity;
    public bool isGrounded;
    public bool isCrouched;
    #endregion
    void Start()
    {
        speed = walkSpeed;
        playerRB = GetComponent<Rigidbody>();
        staminaBar.maxValue = staminaMax;

        SprintCountdown = staminaMax;

        GameObject AudioManager = GameObject.Find("Audio Manager");
        manager = AudioManager.GetComponent < AudioManager > ();
    }
    // Update is called once per frame
    void Update()
    {
        WallRunning();
        isGroundCheck();
        CrouchCheck();
        SpeedChanges();
        SprintTimer();
        SprintCooldown();
     
        // change 1/2 by Toby, player cannot move while executing guard
        if (!movementLocked)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
          


        }
        else
        {
            x = 0;
            z = 0;
        }


        if (isWallRunningUp == false) 
        {
            inputVector = new Vector3(x, 0, z);
            inputVector = inputVector.normalized;
            Vector3 velocity = inputVector * speed;
            velocity = transform.TransformDirection(velocity);
            Controller.Move(velocity * Time.deltaTime);
        }

        // change 2/2 by Toby, player cannot jump while executing guard
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && movementLocked == false)
        {
           
            Jump();
        }
      

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);

        staminaBar.value = SprintCountdown;

      
    }

    void isGroundCheck() 
    {

        if (!isCrouched) 
        {
            RaycastHit hitDown;
            Debug.DrawRay(transform.position - new Vector3(0,1,0), -transform.up * 0.3f, Color.magenta, 0.3f);
            if (Physics.Raycast(transform.position - new Vector3(0, 1, 0), -transform.up, out hitDown, 0.3f, groundMask))
            {
                isGrounded = true;
            }
            else if (!isWallRunningSide)
            {
                isGrounded = false;
            }

        }
        
   
        if (isCrouched) 
        {
            RaycastHit hitDown;
            Debug.DrawRay(transform.position - new Vector3(0, 0.3f, 0), -transform.up * 0.3f, Color.magenta, 0.3f);
            if (Physics.Raycast(transform.position - new Vector3(0, 0.3f, 0), -transform.up, out hitDown, 0.3f, groundMask))
            {
                isGrounded = true;
            }
            else if (!isWallRunningSide)
            {
                isGrounded = false;
            }
        }
        




    }


    void Jump()
    {

        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        manager.Play("Jump");
        if (isCrouched)
        {
            isCrouched = false;
            UnCrouch();
        }
    }



    void SprintTimer() 
    {
        if (isSprinting && SprintCountdown > 0) 
        {
            SprintCountdown = SprintCountdown - Time.deltaTime;
        }

        if (!isSprinting && SprintCountdown <= staminaMax) 
        {
            SprintCountdown = SprintCountdown + Time.deltaTime;
        }
    }


    void SprintCooldown() 
    {
        if (SprintCountdown <= 0) 
        {
            canSprint = false;
        }

        if (!canSprint)
        {
            SprintCooldownCount = SprintCooldownCount - Time.deltaTime;
        }

        if (canSprint)
        {
            SprintCooldownCount = 2;
        }

        if (SprintCooldownCount <= 0)
        {
            canSprint = true;
        }
    }


    void SpeedChanges()
    {
        if (isGrounded && !isCrouched)
        {
            if (Input.GetKey(KeyCode.LeftShift) && canSprint && Input.GetKey(KeyCode.W))
            {
                speed = sprintSpeed;
                isSprinting = true;
            }

            else if (!Input.GetKey(KeyCode.LeftControl))
            {
                speed = walkSpeed;
                isSprinting = false;
            }
        }

    }

    void WallRunning()
    {
        RaycastHit hitLeft;
        Debug.DrawRay(transform.position, -transform.right * 1f, Color.green, 0.5f);
    
        if (Physics.Raycast(transform.position, -transform.right, out hitLeft, 1f)) 
        {
            Debug.DrawRay(transform.position, hitLeft.point);
            if (hitLeft.transform.CompareTag("Climbable") && Input.GetKey(KeyCode.W))
            {
                isGrounded = true;
                if (Input.GetKey(KeyCode.Space) && isGrounded == true)
                {
                    Jump();
                }

                else if (speed == sprintSpeed)
                {
                    velocity.y = 0;
                    isWallRunningSide = true;
                }
            }
            else
            {
                return;
            }
        }

        else
        {
            isWallRunningSide = false;
        }


        RaycastHit hitRight;
        Debug.DrawRay(transform.position, transform.right * 1f, Color.red, 0.5f);

        if (Physics.Raycast(transform.position, transform.right, out hitRight, 1f))
        {
            Debug.DrawRay(transform.position, hitRight.point);
            if (hitRight.transform.CompareTag("Climbable") && Input.GetKey(KeyCode.W))
            {
                isGrounded = true;
                if (Input.GetKey(KeyCode.Space) && isGrounded == true)
                {
                    Jump();
                }

                else if (speed == sprintSpeed)
                {
                    velocity.y = 0;
                    isWallRunningSide = true;
                }

            }
            else
            {
                return;
            }
        }

        else 
        {
            isWallRunningSide = false;
        }

        RaycastHit hitForward;
        Debug.DrawRay(transform.position - new Vector3(0, 1, 0), transform.forward * 1f, Color.blue, 0.5f);

        if (Physics.Raycast(transform.position - new Vector3(0, 1, 0), transform.forward, out hitForward, 1f))
        {
            Debug.DrawRay(transform.position, hitForward.point);
            if (hitForward.transform.CompareTag("Climbable") && Input.GetKey(KeyCode.W))
            {
                isWallRunningUp = true;
                isGrounded = true;
                velocity.y = 3;
            }
            else
            {
            }
        }

        else 
        {
            isWallRunningUp = false;
        }
    }

    void CrouchCheck() 
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouched)
            {
                isCrouched = true;
                Crouch();
            }

            else if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouched)
            {
                isCrouched = false;
                UnCrouch();
            }
        }
    }

    void Crouch()
    {
        if (isCrouched)
        {
            speed = crouchSpeed;
            transform.localScale = transform.localScale / 2;
        }

    }

    void UnCrouch()
    {
        if (!isCrouched)
        {
            speed = walkSpeed;
            transform.localScale = transform.localScale * 2;
        }
    }
}
