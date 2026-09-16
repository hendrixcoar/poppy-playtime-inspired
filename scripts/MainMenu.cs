using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI titleText;
    
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        newGameButton.onClick.AddListener(OnNewGameClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        
        continueButton.interactable = SaveSystem.HasSaveData();
    }

    private void OnNewGameClicked()
    {
        Debug.Log("Starting new game...");
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadLevel(1);
        }
    }

    private void OnContinueClicked()
    {
        Debug.Log("Continuing game...");
        if (SaveSystem.HasSaveData())
        {
            SaveSystem.LoadGame();
        }
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Opening settings...");
        // Open settings UI
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
