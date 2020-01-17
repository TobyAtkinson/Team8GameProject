using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float x;
    float z;

    public CharacterController Controller;



    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float wallRunLength = 10f;
    public bool isWallRunningUp;
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

    Vector3 velocity;
    public bool isGrounded;

    void Start()
    {
        speed = walkSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        WallRunning();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        VelocityReset();
        Crouch();
        SpeedChanges();

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (isWallRunningUp == false) 
        {
            move = transform.right * x + transform.forward * z;
            Controller.Move(move * speed * Time.deltaTime);
        }
        

        Jump();

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }

    void VelocityReset()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }


    void SpeedChanges()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = sprintSpeed;
            }

            else if (!Input.GetKey(KeyCode.LeftControl))
            {
                speed = walkSpeed;
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
                if (speed == sprintSpeed)
                {
                    velocity.y = 0;
                }
            }
            else
            {
                return;
            }
        }

        RaycastHit hitRight;
        Debug.DrawRay(transform.position, transform.right * 1f, Color.red, 0.5f);

        if (Physics.Raycast(transform.position, transform.right, out hitRight, 1f))
        {
            Debug.DrawRay(transform.position, hitRight.point);
            if (hitRight.transform.CompareTag("Climbable") && Input.GetKey(KeyCode.W))
            {
                if (speed == sprintSpeed) 
                { 
                    velocity.y = 0; 
                }
            }
            else 
            {
                return;
            }
        }

        RaycastHit hitForward;
        Debug.DrawRay(transform.position - new Vector3(0, 1, 0), transform.forward * 1f, Color.blue, 0.5f);

        if (Physics.Raycast(transform.position - new Vector3(0, 1, 0), transform.forward, out hitForward, 1f))
        {
            Debug.DrawRay(transform.position, hitForward.point);
            if (hitForward.transform.CompareTag("Climbable") && Input.GetKey(KeyCode.W))
            {
                isWallRunningUp = true;
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

    void Crouch() 
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded) 
        {
            speed = crouchSpeed;
            transform.localScale = transform.localScale / 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isGrounded)
        {
            speed = walkSpeed;
            transform.localScale = transform.localScale * 2;
        }
    }
}
