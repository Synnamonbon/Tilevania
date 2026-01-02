using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int playerScore = 0;
    [SerializeField] private float sceneResetTimer = 1f;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

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
            Invoke("ResetGameSession", sceneResetTimer);
        }
    }

    private void ResetGameSession()
    {
        int sceneIndex = 0;

        FindFirstObjectByType<ScenePersist>().DestroyScenePersist();
        SceneManager.LoadScene(sceneIndex);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        playerLives --;
        livesText.text = playerLives.ToString();

        StartCoroutine(ResetScene(currentSceneIndex));
    }

    private IEnumerator ResetScene(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(sceneResetTimer);

        SceneManager.LoadScene(sceneIndex);
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
