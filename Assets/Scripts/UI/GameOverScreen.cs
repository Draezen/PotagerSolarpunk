using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class GameOverScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("Score details")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI bestScoreLabel;

    [Header("Buttons details")]
    [SerializeField] private GameObject targetButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Display()
    {
        ShowCanvasGroup();
        DisplayScore();
        FocusButton();
    }

    private void ShowCanvasGroup()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void FocusButton() => EventSystem.current.SetSelectedGameObject(targetButton);

    private void DisplayScore()
    {
        int bestScore = scoreManager.bestScore;
        int currentScore = scoreManager.score;

        if (currentScore > bestScore)
            bestScoreLabel.text = "New Record: " + currentScore;
        else
            bestScoreLabel.text = "Best Score: " + bestScore;
    }

}
