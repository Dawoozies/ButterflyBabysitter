using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interaction : MonoBehaviour, Interactable
{
    public string lookAtText = "to interact";
    public UnityEvent<Transform, Vector2> onInteractStart;
    public UnityEvent<Transform, Vector2> onInteractContinue;
    public UnityEvent<Transform, Vector2> onInteractStop;
    public virtual string Look()
    {
        return $"Press {InputManager._interactionKeyText()} {lookAtText}";
    }
    public virtual void InteractStart(Transform interactor, Vector2 mouseDelta)
    {
        onInteractStart?.Invoke(interactor, mouseDelta);
    }
    public virtual void InteractContinue(Transform interactor, Vector2 mouseDelta)
    {
        onInteractContinue?.Invoke(interactor, mouseDelta);
    }
    public virtual void InteractStop(Transform interactor, Vector2 mouseDelta)
    {
        onInteractStop?.Invoke(interactor, mouseDelta);
    }
}
public interface Interactable
{
    public string Look();
    public void InteractStart(Transform interactor, Vector2 mouseDelta);
    public void InteractContinue(Transform interactor, Vector2 mouseDelta);
    public void InteractStop(Transform interactor, Vector2 mouseDelta);
}
public enum Powerup
{
    Flight
}