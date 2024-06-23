using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JumpsLeft : MonoBehaviour
{
    PlayerController playerController;
    TextMeshProUGUI textMesh;
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        textMesh.text = $"Jumps Left: {playerController.jumpsLeft}";
    }
}
