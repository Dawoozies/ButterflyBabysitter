using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    void Awake()
    {
        ins = this;
    }
    public float maxTime;
    float timeLeft;
    public Color DayColor,NightColor;
    public AnimationCurve colorCurve;
    public Camera mainCamera;
    public Light sunLight;
    public Color sunLightDay, sunLightNight;
    public int butterfliesLeft;
    public TextMeshProUGUI butterflyText;
    PlayerController playerController;
    void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
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
        butterflyText.text = $"Butterflies Left: {butterfliesLeft}";
    }
    public void SaveButterfly()
    {
        playerController.jumps++;
        butterfliesLeft--;
    }
    public void RegisterButterfly()
    {
        butterfliesLeft++;
    }
    public void ActivateFlightTime(float flightTime)
    {
        playerController.ActivateFlight(flightTime);
    }
}
