using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform playerTransform;
    public int score;
    public int targetScore = 5;
    public int maxHealth = 5;
    public int currentHealth;
    public float timeRemaining = 90f;
    public float fallThreshold = -5f;
    public bool isGameOver;
    public string message = "Collect coins and reach the goal.";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (playerTransform != null && playerTransform.position.y < fallThreshold)
        {
            Lose("You fell! Press R to restart.");
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            Lose("Time's up! Press R to restart.");
        }

        UIManager.Instance?.UpdateTimer(timeRemaining);

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver)
            return;

        score += amount;
        score = Mathf.Max(score, 0);
        UIManager.Instance?.UpdateScore(score, targetScore);

        if (score >= targetScore)
        {
            Win("Goal reached! Press R to restart.");
        }
        else
        {
            message = $"Collect {targetScore - score} more items.";
            UIManager.Instance?.ShowMessage(message);
        }
    }

    public void Damage(int amount)
    {
        if (isGameOver)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UIManager.Instance?.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Lose("You died! Press R to restart.");
        }
        else
        {
            message = $"Ouch! {currentHealth} health left.";
            UIManager.Instance?.ShowMessage(message);
        }
    }

    public void Win(string winMessage)
    {
        if (isGameOver)
            return;

        isGameOver = true;
        message = winMessage;
        UIManager.Instance?.ShowMessage(message);
    }

    public void Lose(string loseMessage)
    {
        if (isGameOver)
            return;

        isGameOver = true;
        message = loseMessage;
        UIManager.Instance?.ShowMessage(message);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUI()
    {
        UIManager.Instance?.UpdateScore(score, targetScore);
        UIManager.Instance?.UpdateHealth(currentHealth, maxHealth);
        UIManager.Instance?.UpdateTimer(timeRemaining);
        UIManager.Instance?.ShowMessage(message);
    }
}
