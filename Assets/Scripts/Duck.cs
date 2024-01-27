using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Duck : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6;
    public float maxSpeed = 8;
    public float maxSpeedNormal = 8;
    public float maxSpeedDash = 8;
    Vector3 velocity;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.5f;
    public float tufsdfsdf= 0.1f;
    public Vector3 boxCastHalfExtend;

    public float gravity = -9.81f;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public AnimationCurve jumpCurve;
    public float jumpEndTime;
    public float time;
    public float yStart;
    public bool isJumping;
    public AnimationCurve jumpMultiplierCurver;
    public float timePressEnd ;
    public float timePress ;
    public float minJumpTime ;
    [FormerlySerializedAs("dashMultiplierCurver")] public AnimationCurve dashCurver;
    public float dashForce;
    public float timeDashEnd ;
    public float timeDash ;
    public float minDashTime ;
    private bool isDashing;
    private bool canDash;
    public float dashDelay;
    public float dashDelay2;
    private Rigidbody rb;

    public KeyCode Dash;
    public KeyCode Jump;
    public string HorizontalAxis;
    public string VerticalAxis;

    private void Start()
    {
        yStart = transform.position.y;
        rb = GetComponent<Rigidbody>();
        canDash = true;
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
        //rb.AddForce(velocity * Time.deltaTime);
        float horizontal = 0;
        float vertical = 0;
        horizontal = Input.GetAxisRaw(HorizontalAxis);
        vertical = Input.GetAxisRaw(VerticalAxis);

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
            if (!isJumping && isGrounded && !isDashing) {
                rb.AddForce(moveDir.normalized * (speed * Time.deltaTime));
            }

        }

        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        if (Input.GetKey(Jump)) {
            timePress += Time.deltaTime;
        }
        if (Input.GetKeyUp(Jump) && !isJumping && isGrounded && !isDashing) {
            isJumping = true;
            timePress = Mathf.Clamp(timePress, 0.01f, timePressEnd);

        }
        if (Input.GetKey(Dash)) {
            timeDash += Time.deltaTime;
            isDashing = true;
            maxSpeed = maxSpeedDash;
        }
        if (Input.GetKeyUp(Dash) && !isJumping && isGrounded && canDash) {
            canDash = false;
            timeDash = Mathf.Clamp(timeDash, 0.01f, timeDashEnd);
            rb.AddForce(transform.forward * (dashCurver.Evaluate(timeDash/timeDashEnd) * dashForce), ForceMode.Acceleration);

            StartCoroutine(DashDelay());
        }
        if(isJumping) {
            float j = jumpEndTime * (timePress / timePressEnd);
            j = Mathf.Clamp(j, minJumpTime, jumpEndTime);
            if (time < j) {
                time += Time.deltaTime;
                time = Mathf.Clamp(time, 0, j);
                transform.position = new Vector3(transform.position.x, yStart + (jumpCurve.Evaluate(time/j) * jumpMultiplierCurver.Evaluate(timePress/timePressEnd)), transform.position.z);
            }
            else {
                time = 0;
                timePress = 0;
                isJumping = false;
            }

        }
    }


    IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(dashDelay);
        maxSpeed = maxSpeedNormal;
        isDashing = false;
        yield return new WaitForSeconds(dashDelay2);
        canDash = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, boxCastHalfExtend*2);
    }
}
