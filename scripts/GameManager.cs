using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private bool isGamePaused = false;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float gameStartTime;
    
    private int playerHealth = 100;
    private int sanityLevel = 100;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameStartTime = Time.time;
        Debug.Log("Game Started");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        Debug.Log(isGamePaused ? "Game Paused" : "Game Resumed");
    }

    public void DamagePlayer(int damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Max(0, playerHealth);
        Debug.Log($"Player Health: {playerHealth}");
        
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void ReduceSanity(int amount)
    {
        sanityLevel -= amount;
        sanityLevel = Mathf.Max(0, sanityLevel);
        Debug.Log($"Sanity Level: {sanityLevel}");
        
        if (sanityLevel <= 0)
        {
            GameOver();
        }
    }

    public void HealPlayer(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Min(100, playerHealth);
        Debug.Log($"Player Health: {playerHealth}");
    }

    public void RestoreSanity(int amount)
    {
        sanityLevel += amount;
        sanityLevel = Mathf.Min(100, sanityLevel);
        Debug.Log($"Sanity Level: {sanityLevel}");
    }

    public void LoadLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        SceneManager.LoadScene($"Level_{levelNumber}");
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }

    public int GetPlayerHealth() => playerHealth;
    public int GetSanityLevel() => sanityLevel;
    public int GetCurrentLevel() => currentLevel;
    public bool GetPauseState() => isGamePaused;
}
