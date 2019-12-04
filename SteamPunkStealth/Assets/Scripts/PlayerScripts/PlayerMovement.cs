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
    #region Speeds
    public float speed;
    public float walkSpeed;
    public float sprintSpeed;
    public float airSpeed;
    #endregion 



    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

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

        SpeedChanges();

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;

        Controller.Move(move * speed * Time.deltaTime);

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
        if (!isGrounded)
        {
            speed = airSpeed;
        }

        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = sprintSpeed;
            }

            else
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
            if (hitLeft.transform.CompareTag("Wall"))
            {
                float LeftHeightClimbed = 0;

                LeftHeightClimbed += transform.position.y;
                Debug.Log(LeftHeightClimbed);
                Debug.Log("Hit Wall Left");
                velocity.y = (LeftHeightClimbed = -0.1f * Mathf.Pow((wallRunLength + -4.5f), 2) + 2);
                Debug.Log(velocity.y);
            }
        }

        RaycastHit hitRight;
        Debug.DrawRay(transform.position, transform.right * 1f, Color.red, 0.5f);

        if (Physics.Raycast(transform.position, transform.right, out hitRight, 1f))
        {
            Debug.DrawRay(transform.position, hitRight.point);
            if (hitRight.transform.CompareTag("Wall"))
            {
                float LeftHeightClimbed = 0;

                LeftHeightClimbed += transform.position.y;

                Debug.Log("Hit Wall Left");
                velocity.y = (LeftHeightClimbed = -0.1f * Mathf.Pow((wallRunLength + -4.5f), 2) + 2);
                Debug.Log(velocity.y);
            }
        }
    }


}
