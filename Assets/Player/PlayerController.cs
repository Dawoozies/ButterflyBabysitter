using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    public Transform cameraPivot;
    Vector3 cameraPivotOriginPos;
    public float cameraSensitivity;
    public float yRotationLimit;
    public float moveSpeed;
    public float sprintMultiplier;
    bool sprintInput;
    Vector2 angles;

    public float stepSpeed;
    public float sprintStepSpeed;
    float step;
    public UnityEvent<float> onStepTaken;
    Vector3 lastFramePos;

    float _lastFinalMoveSpeed;

    public float jumpMaxVelocity;
    public AnimationCurve jumpCurve;
    public Vector3 jumpDir;
    public float jumpIncreaseSpeed;
    private float jumpValue;
    private bool jumpLetGo;

    private Rigidbody rb;
    public LayerMask groundLayerMask;
    private bool jump;
    private bool applyGravity;

    public float drag;
    Vector2 moveInput;
    Vector3 finalVelocity;
    public Vector3 gravity;
    public Vector3 jumpVelocity;
    public GroundedState groundedState = GroundedState.Falling;
    public float groundExitTime;
    float groundExitTimer;

    bool jumping;
    public float jumpLerpSpeed;
    float jump_t;

    public float flightTimeMax;
    public float flightTime;
    public int jumpsLeft;
    public int jumps = 1;

    public float glideInput;
    public float glideGravityReduction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InputManager.RegisterMouseInputCallback(CameraRotationHandler);
        InputManager.RegisterMoveInputCallback(CharacterMovementHandler);
        lastFramePos = transform.position;

        cameraPivotOriginPos = cameraPivot.localPosition;
        
        InputManager.RegisterJumpInputCallback(JumpInputHandler);
    }
    void JumpInputHandler(bool jumpDownThisFrame, bool jumpHeld, bool jumpUpThisFrame)
    {
        if(jumpDownThisFrame && jumpsLeft > 0 && jumpHeld)
        {
            jump_t = 0f;
            jumpsLeft--;
        }
        if(jumpHeld)
        {
            glideInput += Time.deltaTime;
        }
        else
        {
            glideInput -= Time.deltaTime*4f;
        }
        glideInput = Mathf.Clamp01(glideInput);
    }
    void CameraRotationHandler(Vector2 mouseScreenPos, Vector2 mouseWorldPos, Vector2 mouseDelta)
    {
        RotateCamera(mouseDelta*cameraSensitivity);
        angles.y = Mathf.Clamp(angles.y, -yRotationLimit, yRotationLimit);
        Quaternion xRot = Quaternion.AngleAxis(angles.x, Vector3.up);
        Quaternion yRot = Quaternion.AngleAxis(angles.y, Vector3.left);
        cameraPivot.transform.localRotation = xRot * yRot;
    }
    void CharacterMovementHandler(Vector2 moveInput, bool sprintInput)
    {
        this.moveInput = moveInput;
        this.sprintInput = sprintInput;
    }
    void Update()
    {
        if(groundedState == GroundedState.Exit)
        {
            if(groundExitTimer < groundExitTime)
            {
                groundExitTimer += Time.deltaTime;
            }
            else
            {
                groundedState = GroundedState.Falling;
            }
        }

        if(jump_t < 1)
        {
            jump_t += Time.deltaTime * jumpLerpSpeed;
        }

        if(groundedState == GroundedState.Falling && flightTime <= 0)
        {
            rb.drag = 0f;
        }
        else
        {
            rb.drag = drag;
        }

        if(flightTime > 0)
        {
            flightTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        finalVelocity = Vector3.zero;

        Vector3 projForward = cameraPivot.forward;
        projForward.y = 0f;
        projForward.Normalize();
        Vector3 projRight = cameraPivot.right;
        projRight.y = 0f;
        projRight.Normalize();
        float finalSpeed = moveSpeed;
        if(sprintInput)
        {
            finalSpeed *= sprintMultiplier;
        }
        Vector3 movementVelocity = (projForward * moveInput.y + projRight * moveInput.x) * Time.fixedDeltaTime * finalSpeed;

        movementVelocity.y = 0f;
        finalVelocity += movementVelocity;

        if(groundedState == GroundedState.Falling && jump_t >= 1)
        {
            finalVelocity += (gravity * Time.fixedDeltaTime) * (Mathf.Lerp(1f, glideGravityReduction, glideInput));
        }

        finalVelocity += jumpVelocity * jumpCurve.Evaluate(jump_t) * Time.fixedDeltaTime;

        if (flightTime > 0)
        {
            if(sprintInput)
            {
                finalVelocity = cameraPivot.forward * moveSpeed * sprintMultiplier;
            }
            else
            {
                finalVelocity = cameraPivot.forward * moveSpeed;
            }
        }
        rb.AddForce(finalVelocity, ForceMode.VelocityChange);
    }

    public void RotateCamera(Vector2 screenMoveDelta)
    {
        angles.x += screenMoveDelta.x;
        angles.y += screenMoveDelta.y;
    }
    public void CameraShake(Vector3 shakeMove)
    {
        cameraPivot.localPosition = cameraPivotOriginPos + shakeMove;
    }
    public float MovementValue()
    {
        return Mathf.InverseLerp(0f, moveSpeed*sprintMultiplier, _lastFinalMoveSpeed);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Ground")
        {
            if(Physics.Raycast(transform.position, Vector3.down, 1.05f, groundLayerMask))
            {
                groundedState = GroundedState.Enter;
                groundExitTimer = 0f;
                jumpsLeft = jumps;
            }
        }
    }
    private void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "Ground")
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1.05f, groundLayerMask))
            {
                groundedState = GroundedState.Stay;
                groundExitTimer = 0f;
                jumpsLeft = jumps;
            }
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if(col.collider.tag == "Ground")
        {
            groundedState = GroundedState.Exit;
        }
    }
    public void ActivateFlight(float flightTime)
    {
        this.flightTime = flightTime;
        flightTimeMax = flightTime;
    }
}
public enum GroundedState
{
    Falling, Enter, Stay, Exit
}
public enum JumpInputState
{
    Down, Held, Up
}