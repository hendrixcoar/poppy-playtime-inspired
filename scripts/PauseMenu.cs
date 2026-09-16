using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI pauseTitle;
    
    private bool isPaused = false;

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.alpha = isPaused ? 1f : 0f;
            pauseMenuPanel.interactable = isPaused;
            pauseMenuPanel.blocksRaycasts = isPaused;
        }
        
        if (GameManager.instance != null)
        {
            if (isPaused)
                GameManager.instance.TogglePause();
            else
                GameManager.instance.TogglePause();
        }
    }

    private void OnResumeClicked()
    {
        TogglePause();
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Opening settings from pause menu...");
        // Open settings UI
    }

    private void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
