using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    void Start()
    {
        GameManager.ins.RegisterButterfly();
    }
    public void InteractStartHandler(Transform interactor, Vector2 mouseDelta)
    {
        GameManager.ins.SaveButterfly();
        gameObject.SetActive(false);
    }
}
