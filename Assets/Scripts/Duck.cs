using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6;
    public float maxSpeed = 8;
    Vector3 velocity;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.5f;
    public float tufsdfsdf= 0.1f;
    public Vector3 boxCastHalfExtend;

    public float gravity = -9.81f;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public AnimationCurve jumpCurve;
    public float jumpEndTime;
    public float time;
    public float yStart;
    public bool isJumping;

    private Rigidbody rb;

    private void Start()
    {
        yStart = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = Physics.BoxCast(groundCheck.position, boxCastHalfExtend, Vector3.down, transform.rotation, float.MaxValue, groundMask);
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);
        rb.AddForce(velocity * Time.deltaTime);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 moveDir = Vector3.Lerp(transform.forward, direction.normalized, tufsdfsdf);
        if (direction.magnitude >= 0.1f )
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
//
            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDir.normalized * (speed * Time.deltaTime));
            rb.AddForce(moveDir.normalized * (speed * Time.deltaTime));

        }

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            isJumping = true;
        }




        if(isJumping){
            if (time < jumpEndTime) {
                time += Time.deltaTime;
                time = Mathf.Clamp(time, 0, jumpEndTime);
                transform.position = new Vector3(transform.position.x, yStart + jumpCurve.Evaluate(time/jumpEndTime), transform.position.z);
            }
            else {
                time = 0;
                isJumping = false;
            }

        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, boxCastHalfExtend*2);
    }
}
