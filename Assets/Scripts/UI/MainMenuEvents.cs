using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private VisualElement root;
    private Label scoreLabel;
    private Button playButton;
    private Button quitButton;
    private int bestScore;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        scoreLabel = root.Q<Label>("Menu__BestScoreLabel");
        playButton = root.Q<Button>("Menu__PlayBtn");
        quitButton = root.Q<Button>("Menu__QuitBtn");
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("bestScore", defaultValue: 0);
        UpdateBestScoreDisplay();
    }

    private void OnEnable()
    {
        playButton.clicked += OnPlayClicked;
        quitButton.clicked += OnQuitClicked;
    }

    private void OnDisable()
    {
        playButton.clicked -= OnPlayClicked;
        quitButton.clicked -= OnQuitClicked;
    }

    public void OnPlayClicked() => SceneLoader.instance.LoadByScene(SceneList.Level);

    public void OnQuitClicked() => Application.Quit();

    private void UpdateBestScoreDisplay() => scoreLabel.text = $"Best score : {bestScore}";
}
