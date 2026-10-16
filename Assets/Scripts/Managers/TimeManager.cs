using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Game details")]
    [SerializeField] private float gameTime;
    [SerializeField] private TextMeshProUGUI timeLabel;

    private GameManager gameManager;
    private AudioSource clockAS;

    private bool isGameOver = false;
    private bool isEndOfTimerTriggered = false;
    private float timeLeft;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        clockAS = GetComponentInChildren<AudioSource>();

        timeLeft = gameTime;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if (timeLeft <= 0)
                GameOver();
            else
                UpdateTime();
        }
    }

    private void GameOver()
    {
        timeLeft = 0;
        isGameOver = true;
        clockAS.Stop();
        gameManager.SetGameOver();
    }

    private void UpdateTime()
    {
        timeLeft -= Time.deltaTime;

        if (!isEndOfTimerTriggered && timeLeft <= 10.0)
        {
            isEndOfTimerTriggered = true;
            clockAS.Play();
        }

        UpadteTimeDisplay();
    }

    private void UpadteTimeDisplay()
    {
        string formatedTime = FormatTime();

        timeLabel.text = isEndOfTimerTriggered ? $"<color=red>{formatedTime}</color>" : formatedTime;
    }

    private string FormatTime()
    {
        int minutes = Mathf.FloorToInt(Mathf.Max(timeLeft / 60, 0));
        int seconds = (int)timeLeft % 60;

        return string.Format("{0:00}:{1:00}", minutes, seconds); ;
    }
}
