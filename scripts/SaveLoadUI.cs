using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI saveStatusText;
    [SerializeField] private TextMeshProUGUI saveInfoText;
    [SerializeField] private CanvasGroup saveLoadPanel;
    
    private void Start()
    {
        saveButton.onClick.AddListener(OnSaveClicked);
        loadButton.onClick.AddListener(OnLoadClicked);
        deleteButton.onClick.AddListener(OnDeleteClicked);
        
        UpdateSaveInfo();
        
        if (saveLoadPanel != null)
        {
            saveLoadPanel.alpha = 0f;
        }
    }

    private void OnSaveClicked()
    {
        SaveSystem.SaveGame();
        ShowStatusMessage("Game saved successfully!", Color.green);
        UpdateSaveInfo();
    }

    private void OnLoadClicked()
    {
        if (SaveSystem.LoadGame())
        {
            ShowStatusMessage("Game loaded successfully!", Color.green);
        }
        else
        {
            ShowStatusMessage("Failed to load game!", Color.red);
        }
    }

    private void OnDeleteClicked()
    {
        SaveSystem.DeleteSaveData();
        ShowStatusMessage("Save data deleted!", Color.yellow);
        UpdateSaveInfo();
    }

    private void ShowStatusMessage(string message, Color color)
    {
        if (saveStatusText != null)
        {
            saveStatusText.text = message;
            saveStatusText.color = color;
        }
    }

    private void UpdateSaveInfo()
    {
        if (saveInfoText != null)
        {
            saveInfoText.text = SaveSystem.GetSaveInfo();
        }
        
        loadButton.interactable = SaveSystem.HasSaveData();
        deleteButton.interactable = SaveSystem.HasSaveData();
    }

    public void ToggleSaveLoadPanel()
    {
        if (saveLoadPanel != null)
        {
            saveLoadPanel.alpha = saveLoadPanel.alpha > 0.5f ? 0f : 1f;
            saveLoadPanel.interactable = !saveLoadPanel.interactable;
            saveLoadPanel.blocksRaycasts = !saveLoadPanel.blocksRaycasts;
        }
    }
}
