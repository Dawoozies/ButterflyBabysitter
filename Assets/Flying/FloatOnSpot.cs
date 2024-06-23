using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOnSpot : MonoBehaviour
{
    public AnimationCurve curve;
    float t;
    public float frequency;
    Vector3 origin;
    public Vector3 floatEndPoint;
    private void Start()
    {
        origin = transform.localPosition;
    }
    void Update()
    {
        t += Time.deltaTime * frequency;
        transform.localPosition = Vector3.LerpUnclamped(origin, floatEndPoint, curve.Evaluate(t));
    }
}
