using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public Vector2 heightBounds;
    Vector3 originPos;
    Vector3 startPos;
    Vector3 endPos;
    float t;
    public AnimationCurve lerpCurve;
    public float speed;
    private void Start()
    {
        originPos = transform.position;
    }
    void Update()
    {
        startPos = originPos;
        endPos = originPos;
        startPos.y += heightBounds.x;
        endPos.y += heightBounds.y;

        t += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(startPos, endPos, lerpCurve.Evaluate(t));
    }
}
