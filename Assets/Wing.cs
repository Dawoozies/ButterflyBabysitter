using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour
{
    public float wingSpeed;
    public AnimationCurve lerpCurve;
    public Vector3 rotStart;
    public Vector3 rotEnd;
    float t;
    void Update()
    {
        t += Time.deltaTime * wingSpeed;
        transform.localRotation = Quaternion.Euler(Vector3.SlerpUnclamped(rotStart,rotEnd, lerpCurve.Evaluate(t)));
    }
}
