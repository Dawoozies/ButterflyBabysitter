using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCrystal : Interaction
{
    public float flightTime;
    PlayerController playerController;
    public override string Look()
    {
        return $"Press {InputManager._interactionKeyText()} to gain {flightTime} seconds of flight";
    }
    public override void InteractStart(Transform interactor, Vector2 mouseDelta)
    {
        if(playerController == null)
        {
            playerController = interactor.GetComponent<PlayerController>();
        }

        if(playerController != null)
        {
            playerController.ActivateFlight(flightTime);
        }

        base.InteractStart(interactor, mouseDelta);
    }
}
