using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    float radius => transform.localScale.z/2f;
    public int eyeCount = 40;
    public GameObject prefab;
    Transform[] eyes;
    public Vector2 rightShiftBounds;
    public Vector2 upShiftBounds;
    public Vector2 scaleBounds;
    void Start()
    {
        eyes = new Transform[eyeCount];
        for (int i = 0; i < eyeCount; i++)
        {
            eyes[i] = Instantiate(prefab, transform).transform;
            Vector3 randomUp = transform.up * Random.Range(upShiftBounds.x, upShiftBounds.y);
            Vector3 randomRight = transform.right * Random.Range(rightShiftBounds.x, rightShiftBounds.y);

            eyes[i].position = transform.position + (transform.forward + randomUp + randomRight).normalized * radius;
            eyes[i].localScale = Vector3.one*Random.Range(scaleBounds.x,scaleBounds.y);
        }
    }
}
