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
}
