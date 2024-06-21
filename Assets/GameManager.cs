using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxTime;
    float timeLeft;
    public Color DayColor,NightColor;
    public AnimationCurve colorCurve;
    public Camera mainCamera;
    public Light sunLight;
    public Color sunLightDay, sunLightNight;
    void Start()
    {
        timeLeft = maxTime;
    }
    void Update()
    {
        mainCamera.backgroundColor = Color.Lerp(NightColor,DayColor, colorCurve.Evaluate(timeLeft/maxTime));
        sunLight.color = Color.Lerp(sunLightNight, sunLightDay, colorCurve.Evaluate(timeLeft / maxTime));
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            timeLeft = 0;
        }
    }
}
