using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public KeyCode releaseCursorDebug;
    bool cursorLocked;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }
    void Update()
    {
        if(Input.GetKeyDown(releaseCursorDebug))
        {
            cursorLocked = !cursorLocked;
        }
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Cursor.visible = !cursorLocked;
    }
}
