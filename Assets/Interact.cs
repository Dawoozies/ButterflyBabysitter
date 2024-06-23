using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Interact : MonoBehaviour
{
    Camera worldCamera;
    public float distance;
    public LayerMask interactLayers;
    Vector2 screen;
    TextMeshProUGUI interactionText;
    Interactable interactable;
    Vector2 mouseDelta;
    void Start()
    {
        interactionText = GameObject.FindWithTag("InteractText").GetComponent<TextMeshProUGUI>();
        worldCamera = Camera.main;
        InputManager.RegisterMouseInputCallback(MouseInputHandler);
        InputManager.RegisterInteractionInputCallback(InteractionInputHandler);
    }

    private void InteractionInputHandler(int inputState, float heldTime)
    {
        if (interactable == null)
            return;
        switch (inputState)
        {
            case 0:
                interactable.InteractStart(transform, mouseDelta);
                break;
            case 1:
                interactable.InteractContinue(transform, mouseDelta);
                break;
            case 2:
                interactable.InteractStop(transform, mouseDelta);
                break;
            default:
                break;
        }
    }
    private void MouseInputHandler(Vector2 screen, Vector2 world, Vector2 mouseDelta)
    {
        this.screen = screen;
        this.mouseDelta = mouseDelta;
    }
    private void Update()
    {

        Ray ray;
        ray = worldCamera.ScreenPointToRay(screen);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, interactLayers))
        {
            interactable = hit.collider.GetComponentInParent<Interactable>();
        }
        else
        {
            interactable = null;
        }

        if (interactionText == null)
            return;
        if (interactable != null)
        {
            interactionText.text = interactable.Look();
        }
        else
        {
            interactionText.text = "";
        }
    }
}