using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private string levelName = "Level 1";
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private string levelDescription = "";
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Room[] rooms;
    [SerializeField] private Vector3 levelBounds = new Vector3(100, 50, 100);
    
    [Header("Level Settings")]
    [SerializeField] private int enemyDifficulty = 1;
    [SerializeField] private bool hasTimer = false;
    [SerializeField] private float timeLimit = 300f;
    private float timeRemaining;
    
    [Header("Completion")]
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private int requiredPuzzlesToComplete = 1;
    private int puzzlesCompleted = 0;

    private void Start()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        Debug.Log($"Initializing Level: {levelName}");
        
        if (playerSpawnPoint != null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.transform.position = playerSpawnPoint.position;
                player.transform.rotation = playerSpawnPoint.rotation;
            }
        }
        
        timeRemaining = timeLimit;
        puzzlesCompleted = 0;
        isCompleted = false;
    }

    private void Update()
    {
        if (hasTimer && !isCompleted)
        {
            timeRemaining -= Time.deltaTime;
            
            if (timeRemaining <= 0)
            {
                OnTimeLimitExceeded();
            }
        }
    }

    private void OnTimeLimitExceeded()
    {
        Debug.Log($"Time limit exceeded on {levelName}!");
        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }

    public void OnPuzzleCompleted()
    {
        puzzlesCompleted++;
        Debug.Log($"Puzzle completed! {puzzlesCompleted}/{requiredPuzzlesToComplete}");
        
        if (puzzlesCompleted >= requiredPuzzlesToComplete)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        isCompleted = true;
        Debug.Log($"Level Complete: {levelName}");
        
        // Trigger level complete UI/events
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void LoadNextLevel()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadLevel(levelNumber + 1);
        }
    }

    public string GetLevelName() => levelName;
    public int GetLevelNumber() => levelNumber;
    public Room[] GetRooms() => rooms;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsLevelCompleted() => isCompleted;
}
