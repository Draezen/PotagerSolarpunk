using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private TextMeshProUGUI scoreLabel;

    public int score { get; private set; }
    public int bestScore { get; private set; }
    private Coroutine currentCoroutine;

    private void Start()
    {
        ResetScoreDisplay();
        bestScore = PlayerPrefs.GetInt("bestScore", defaultValue: 0);
    }

    public void SaveBestScore()
    {
        if (score > bestScore)
            PlayerPrefs.SetInt("bestScore", score);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreDisplay();
    }

    private void ResetScoreDisplay() => scoreLabel.text = "0";

    private void UpdateScoreDisplay()
    {
        scoreLabel.text = score.ToString();
        ExpandScoreText();
    }

    private void ExpandScoreText()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        StartCoroutine(ExpandScoreTextCo());
    }

    private IEnumerator ExpandScoreTextCo()
    {
        scoreLabel.color = Color.yellow;
        scoreLabel.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        yield return new WaitForSeconds(.3f);

        scoreLabel.color = Color.white;
        scoreLabel.rectTransform.localScale = Vector3.one;
    }
}
