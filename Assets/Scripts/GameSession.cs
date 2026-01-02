using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int playerScore = 0;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int levelStartScore;

    private void Awake()
    {
        int numberGameSession = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        if (numberGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        playerLives --;
        livesText.text = playerLives.ToString();
        ResetLevelScore();
        scoreText.text = playerScore.ToString();
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ResetLevelScore()
    {
        playerScore = levelStartScore;
    }

    public void SetStartScore()
    {
        levelStartScore = playerScore;
    }

    public void PickupCoin(int coinValue)
    {
        AddScore(coinValue);
    }

    private void AddScore(int coinValue)
    {
        playerScore += coinValue;
        scoreText.text = playerScore.ToString();
    }
}
