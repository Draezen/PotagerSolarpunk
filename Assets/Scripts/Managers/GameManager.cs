using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private DroneMovement drone;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        musicAudioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        musicAudioSource.Play();
    }

    public void SetGameOver()
    {
        scoreManager.SaveBestScore();
        drone.gameObject.SetActive(false);
        gameOverScreen.Display();
    }
}
