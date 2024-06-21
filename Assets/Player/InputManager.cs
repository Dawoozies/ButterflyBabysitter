using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static List<Action<Vector2, Vector2, Vector2>> mouseInputActions = new();
    public static List<Action<Vector2, bool>> moveInputActions = new();
    public static List<Action<float>> mouseLeftClickActions = new();
    float leftClickHeldTime = 0f;
    public KeyCode reloadKey;
    float reloadInputBuffer;
    public static List<Action<float>> reloadInputActions = new();
    public static List<Action<float>> mouseRightClickActions = new();
    float rightClickHeldTime = 0f;
    public static List<Action<float>> scrollWheelInputActions = new();
    public float scrollWheelDeadzone;

    //Interaction
    public static List<Action<int, float>> interactionInputActions = new();
    public KeyCode interactionKey;
    public static KeyCode _interactionKey;
    float interactionHeldTime = 0f;

    //sprint key
    public KeyCode sprintKey;
    public static List<Action<int, float>> alphaNumericKeyInputActions = new();
    int _alphaNumericInput = -1;
    float alphaNumericHeldTime;


    public static List<Action<bool, bool, bool>> jumpInputActions = new();
    public static string _interactionKeyText()
    {
        return $"<b><color=#ff0000ff>{_interactionKey}</color></b>";
    }
    public static void ResetInputManager()
    {
        mouseInputActions.Clear();
        moveInputActions.Clear();
        mouseLeftClickActions.Clear();
        reloadInputActions.Clear();
        mouseRightClickActions.Clear();
        scrollWheelInputActions.Clear();
        interactionInputActions.Clear();
        alphaNumericKeyInputActions.Clear();
    }

    void Update()
    {
        _interactionKey = interactionKey;

        Vector2 mouseScreenPos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 mouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if(Cursor.visible)
        {
            mouseDelta = Vector2.zero;
        }
        foreach (Action<Vector2,Vector2,Vector2> action in mouseInputActions)
        {
            action(mouseScreenPos, mouseWorldPos, mouseDelta);
        }
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool sprintInput = Input.GetKey(sprintKey);

        bool jumpDownThisFrame = Input.GetButtonDown("Jump");
        bool jumpHeld = Input.GetButton("Jump");
        bool jumpUpThisFrame = Input.GetButtonUp("Jump");
        foreach (var action in jumpInputActions)
        {
            action(jumpDownThisFrame, jumpHeld, jumpUpThisFrame);
        }
        foreach (var action in moveInputActions)
        {
            action(moveInput, sprintInput);
        }
        foreach (Action<float> action in mouseLeftClickActions)
        {
            action(leftClickHeldTime);
        }
        if (Input.GetMouseButton(0))
        {
            leftClickHeldTime += Time.deltaTime;
        }
        else
        {
            leftClickHeldTime = 0f;
        }

        //Interaction
        bool interactionDown = Input.GetKeyDown(interactionKey);
        bool interactionHeld = Input.GetKey(interactionKey);
        bool interactionUp = Input.GetKeyUp(interactionKey);
        //Debug.Log($"down = {interactionDown} held = {interactionHeld} up = {interactionUp}");
        int interactionKeyState = -1;
        switch ((interactionDown, interactionHeld, interactionUp))
        {
            case (true, true, false):
                //down input
                interactionKeyState = 0;
                break;
            case (false, true, false):
                //held input
                interactionKeyState = 1;
                interactionHeldTime += Time.deltaTime;
                break;
            case (false, false, true):
                interactionKeyState = 2;
                break;
            default:
                interactionHeldTime = 0;
                break;
        }
        foreach (var action in interactionInputActions)
        {
            action(interactionKeyState, interactionHeldTime);
        }


        if (Input.GetKey(reloadKey))
        {
            reloadInputBuffer = 0.45f;
        }
        else
        {
            if(reloadInputBuffer > 0)
            {
                reloadInputBuffer -= Time.deltaTime;
            }
        }
        foreach (Action<float> action in reloadInputActions)
        {
            action(reloadInputBuffer);
        }
        if (Input.GetMouseButton(1))
        {
            rightClickHeldTime += Time.deltaTime;
        }
        else
        {
            rightClickHeldTime = 0f;
        }
        foreach (var action in mouseRightClickActions)
        {
            action(rightClickHeldTime);
        }
        float scrollDelta = Input.mouseScrollDelta.y;
        if(Mathf.Abs(scrollDelta) < scrollWheelDeadzone)
        {
            scrollDelta = 0f;
        }
        foreach(var action in scrollWheelInputActions)
        {
            action(scrollDelta);
        }
        AlphaNumericInputUpdate();
    }
    public void AlphaNumericInputUpdate()
    {
        int alphaNumericInputThisFrame = -1;
        if(Input.GetKey(KeyCode.Alpha0))
        {
            alphaNumericInputThisFrame = 0;
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            alphaNumericInputThisFrame = 1;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            alphaNumericInputThisFrame = 2;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            alphaNumericInputThisFrame = 3;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            alphaNumericInputThisFrame = 4;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            alphaNumericInputThisFrame = 5;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            alphaNumericInputThisFrame = 6;
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            alphaNumericInputThisFrame = 7;
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            alphaNumericInputThisFrame = 8;
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            alphaNumericInputThisFrame = 9;
        }

        if(_alphaNumericInput != alphaNumericInputThisFrame)
        {
            _alphaNumericInput = alphaNumericInputThisFrame;
            alphaNumericHeldTime = 0f;
        }
        else
        {
            alphaNumericHeldTime += Time.deltaTime;
        }

        if(_alphaNumericInput == -1)
        {
            alphaNumericHeldTime = 0f;
        }

        foreach  (var action in alphaNumericKeyInputActions)
        {
            action(_alphaNumericInput, alphaNumericHeldTime);
        }
    }
    public static void RegisterMouseInputCallback(Action<Vector2, Vector2, Vector2> action)
    {
        mouseInputActions.Add(action);
    }
    public static void RegisterMoveInputCallback(Action<Vector2,bool> action)
    {
        moveInputActions.Add(action);
    }
    public static void RegisterMouseLeftClickCallback(Action<float> action)
    {
        mouseLeftClickActions.Add(action);
    }
    public static void RegisterInteractionInputCallback(Action<int, float> action)
    {
        interactionInputActions.Add(action);
    }
    public static void RegisterReloadInputCallback(Action<float> action)
    {
        reloadInputActions.Add(action);
    }
    public static void RegisterMouseRightClickCallback(Action<float> action)
    {
        mouseRightClickActions.Add(action);
    }
    public static void RegisterScrollWheelCallback(Action<float> action)
    {
        scrollWheelInputActions.Add(action);
    }
    public static void RegisterAlphaNumericKeyCallback(Action<int,float> action)
    {
        alphaNumericKeyInputActions.Add(action);
    }

    public static void RegisterJumpInputCallback(Action<bool, bool, bool> action)
    {
        jumpInputActions.Add(action);
    }
}
