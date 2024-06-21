using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private Vector3 g;
    private bool jump;
    private bool applyGravity;
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
        if (jumpDownThisFrame)
        {
            rb.AddForce(jumpDir*jumpMaxVelocity, ForceMode.Impulse);
        }
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
        float finalMoveSpeed = moveSpeed;
        this.sprintInput = sprintInput;
        if(sprintInput)
        {
            finalMoveSpeed *= sprintMultiplier;
        }
        Vector3 projForward = cameraPivot.forward;
        projForward.y = 0f;
        projForward.Normalize();
        Vector3 projRight = cameraPivot.right;
        projRight.y = 0f;
        projRight.Normalize();
        Vector3 v = (projForward * moveInput.y + projRight * moveInput.x) * finalMoveSpeed;
        
        Vector3 dv = v * Time.deltaTime;
        dv.y = 0;
        rb.AddForce(dv, ForceMode.VelocityChange);
        _lastFinalMoveSpeed = v.magnitude;
    }
    void Update()
    {
        applyGravity = !GroundedCheck();
        Vector3 absMovement = transform.position - lastFramePos;
        float absMovementMagnitude = absMovement.magnitude;
        //Debug.LogError($"absMovement = {absMovement} || magnitude = {absMovement.magnitude}");
        if(absMovementMagnitude > 0)
        {
            float stepLoudnessMultiplier = 1f;
            if(sprintInput)
            {
                stepLoudnessMultiplier = 2f;
                step += Time.deltaTime * sprintStepSpeed;
            }
            else
            {
                step += Time.deltaTime * stepSpeed;
            }

            if(step >= 1)
            {
                onStepTaken?.Invoke(stepLoudnessMultiplier);
                step = 0f;
            }
        }
        lastFramePos = transform.position;
    }

    private void FixedUpdate()
    {
        if (applyGravity)
        {
            g += Vector3.down;
            rb.AddForce(g*Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            g = Vector3.zero;
        }
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

    public bool GroundedCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayerMask);
    }
}
