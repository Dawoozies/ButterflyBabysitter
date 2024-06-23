using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public Vector3 rotateAxis;
    void Update()
    {
        transform.Rotate(rotateAxis, 360f*speed*Time.deltaTime);
    }
}
