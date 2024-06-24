using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI timeLeftDisplay;
    public Rigidbody mother;
    public float attackTimeCooldown;
    float attackTime;
    public float attackVelocity;
    public bool beatGame;
    public TextMeshProUGUI objectiveText;
    public GameObject beatGamePanel;
    void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        timeLeft = maxTime;
    }
    void Update()
    {
        objectiveText.gameObject.SetActive(butterfliesLeft <= 0);
        beatGamePanel.SetActive(beatGame);
        if (beatGame)
        {
            mother.velocity = Vector3.zero;
            return;
        }
        mainCamera.backgroundColor = Color.Lerp(NightColor,DayColor, colorCurve.Evaluate(timeLeft/maxTime));
        sunLight.color = Color.Lerp(sunLightNight, sunLightDay, colorCurve.Evaluate(timeLeft / maxTime));
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            timeLeft = 0;
            if(attackTime < attackTimeCooldown)
            {
                attackTime += Time.deltaTime;
            }
            if(attackTime >= attackTimeCooldown)
            {
                MotherAttack();
                attackTime = 0;
            }
        }
        butterflyText.text = $"Butterflies Left: {butterfliesLeft}";
        timeLeftDisplay.text = $"Time Left: {Mathf.Round(timeLeft*100f)/100f}";
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
    void MotherAttack()
    {
        Vector3 dirToPlayer = playerController.transform.position - mother.position;
        mother.velocity = dirToPlayer*attackVelocity;
        mother.transform.forward = dirToPlayer;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
