using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Public Parameters

    [Header("Movement Properties")]
    public float walkSpeed = 12f;
    public float sprintSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    [Header("Footstep Audio Properties")]
    [Range(0f, 1f)] public float sprintStepLength;
    public float stepInterval;
    public AudioClip[] footStepClips;
    public AudioSource source;
    [Header("Ground Check Properties")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    #endregion

    #region Private Parameters

    private CharacterController controller;

    private Vector3 velocity;

    private bool isGrounded;
    //private bool isSprinting;
    private bool isWalking;
    private bool isJumping;

    private CollisionFlags collisionFlags;

    private float horizontalInput;
    private float verticalInput;
    private float stepCycle;
    private float nextStep;

    #endregion

    private void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput(out float speed)
    {
        //Get Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        isJumping = Input.GetButtonDown("Jump");

        isWalking = !Input.GetButton("Fire3");

        speed = isWalking ? walkSpeed : sprintSpeed;
    }

    void Move()
    {
        GetInput(out float speed);

        //Check if Grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //If grounded reset velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Get direction
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        controller.Move(move * speed * Time.deltaTime); 

        //Apply jump if grounded
        if (isJumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        StepProgressCycle(speed);
    }

    //Apply force to physics object when player collides with them
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (collisionFlags == CollisionFlags.Below)
            return;

        if (rb == null || rb.isKinematic)
            return;

        rb.AddForceAtPosition(controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    void StepProgressCycle(float speed)
    {
        if (controller.velocity.sqrMagnitude > 0 && (horizontalInput != 0 || verticalInput != 0))
            stepCycle += (controller.velocity.magnitude + (speed * (isWalking ? 1f : sprintStepLength))) * Time.deltaTime;

        if (!(stepCycle > nextStep))
            return;

        nextStep = stepCycle + stepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if (!isGrounded)
            return;

        int i = UnityEngine.Random.Range(1, footStepClips.Length);
        source.clip = footStepClips[i];
        source.PlayOneShot(source.clip);

        footStepClips[i] = footStepClips[0];
        footStepClips[0] = source.clip;
    }

    void Init()
    {
        if (!controller)
        {
            controller = GetComponent<CharacterController>();
            if (!controller)
                controller = gameObject.AddComponent<CharacterController>();
        }
        if (!source)
        {
            source = GetComponent<AudioSource>();
            if (!source)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
            }   
        }

        stepCycle = 0f;
        nextStep = stepCycle / 2f;
        isJumping = false;
    }
}
