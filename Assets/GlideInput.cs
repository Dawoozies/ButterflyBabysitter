using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GlideInput : MonoBehaviour
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
        slider.maxValue = 1f;
        slider.value = playerController.glideInput;
        fillImage.color = Color.Lerp(minInputColor, maxInputColor, slider.value);
    }
}
