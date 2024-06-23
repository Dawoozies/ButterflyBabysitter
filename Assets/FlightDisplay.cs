using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlightDisplay : MonoBehaviour
{
    PlayerController playerController;
    Slider slider;
    public Color maxInputColor;
    public Color minInputColor;
    public Image fillImage;
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        slider = GetComponent<Slider>();
    }
    private void Update()
    {
        slider.minValue = 0f;
        slider.maxValue = playerController.flightTimeMax;
        slider.value = playerController.flightTime;
        fillImage.color = Color.Lerp(minInputColor, maxInputColor, slider.value/slider.maxValue);
    }
}
