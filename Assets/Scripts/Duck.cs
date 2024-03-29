using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    public float stunDuration;
    private Rigidbody rb;
    private Animator animator;

    public KeyCode Dash;
    public KeyCode Jump;
    public string HorizontalAxis;
    public string VerticalAxis;
    public float collisionForce;
    public float collideDelay;
    private bool isColliding;
    private bool isStunned = false;
    public bool havePlayerFallingSounds = false;
    public Transform hatPivot;
    public int playerNUm;
    public GameObject checkCinematic;
    public float deathDelay;

    public List<AudioClip> DashSounds;
    public List<AudioClip> JumpSounds;
    public List<AudioClip> StunSounds;
    public List<AudioClip> FallingSounds;
    public List<AudioClip> VictorySounds;
    public  AudioSource audioSource;
    public bool end;
    public bool haveWin;
    private void Start()
    {
        yStart = transform.position.y;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        canDash = true;
        Instantiate(HatManager.Instance.TakeHat(), hatPivot);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Player") && !isColliding) {

            StartCoroutine(CollideDelay(collision));
        }
    }


    public void StopFalling()
    {
        animator.SetBool("DeathFall", false);
    }

    private void PlaySoundsRoundom(List<AudioClip> sounds)
    {
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
    }

    void Update()
    {

        if(!checkCinematic.activeSelf || end){return;}
        if (isStunned) {
            canDash = true;
            isDashing = false;
            timeDash = 0;
            time = 0;
            timePress = 0;
            return;
        }
        isGrounded = Physics.BoxCast(groundCheck.position, boxCastHalfExtend, Vector3.down, transform.rotation, float.MaxValue, groundMask);
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


            if (!isGrounded && !havePlayerFallingSounds) {
                animator.SetBool("DeathFall", !isGrounded);
                havePlayerFallingSounds = true;
                PlaySoundsRoundom(FallingSounds);
            }


        //velocity.y += gravity * Time.deltaTime;
        if (!isJumping) {
            rb.AddForce(Vector3.down*gravity);
        }
        //controller.Move(velocity * Time.deltaTime);
        //rb.AddForce(velocity * Time.deltaTime);
        float horizontal = 0;
        float vertical = 0;
        vertical = Input.GetAxisRaw(HorizontalAxis);
        horizontal = -Input.GetAxisRaw(VerticalAxis);

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
            animator.SetBool("LoadingJump", true);
        }
        if (Input.GetKeyUp(Jump) && !isJumping && isGrounded && !isDashing) {
            PlaySoundsRoundom(JumpSounds);
            isJumping = true;
            timePress = Mathf.Clamp(timePress, 0.01f, timePressEnd);
            animator.SetTrigger("Jump");
            animator.SetBool("LoadingJump", false);
        }
        if (Input.GetKey(Dash) && canDash) {
            timeDash += Time.deltaTime;
            isDashing = true;
            animator.SetBool("DashLoading", true);
        }


        if (Input.GetKeyUp(Dash) && !isJumping && isGrounded && canDash) {
            maxSpeed = maxSpeedDash;
            canDash = false;
            timeDash = Mathf.Clamp(timeDash, 0.01f, timeDashEnd);
            PlaySoundsRoundom(DashSounds);
            rb.AddForce(transform.forward * (dashCurver.Evaluate(timeDash/timeDashEnd) * dashForce), ForceMode.Acceleration);
	    timeDash = 0.01f;	
            animator.SetBool("DashLoading", false);
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
        Debug.Log("AAAAAAAA " + isDashing);
        yield return new WaitForSeconds(dashDelay2);
        canDash = true;
    }

    IEnumerator CollideDelay(Collision collision)
    {
        //Debug.Log(collision.transform.name + " Velocita" +  collision.transform.GetComponent<Rigidbody>().velocity.magnitude);
        //float v = Mathf.Abs(collision.transform.GetComponent<Rigidbody>().velocity.magnitude);
        //yield return new WaitForSeconds(1);
        Vector3 dir = transform.position - collision.transform.position;
        dir.Normalize();
        maxSpeed = maxSpeedDash;
        rb.AddForce(dir * collisionForce, ForceMode.Acceleration);

        isColliding = true;
        yield return new WaitForSeconds(collideDelay);
        maxSpeed = maxSpeedNormal;
        isColliding = false;
    }

    public void PlayRandomVicotryAnimation()
    {
        PlaySoundsRoundom(VictorySounds);
        int r = Random.Range(0, 3);
        switch (r) {
            case 0:
                Debug.Log("v1");
                animator.SetTrigger("V1");
                break;
            case 1:
                Debug.Log("v2");
                animator.SetTrigger("V2");
                break;
            case 2:
                Debug.Log("v3");
                animator.SetTrigger("V3");
                break;
        }
    }

    public void BigVicoryAnim()
    {
        animator.SetTrigger("BV");
    }

    public void Stun()
    {
        PlaySoundsRoundom(StunSounds);
        StartCoroutine(StunCoroutine());
    }

    IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
}
