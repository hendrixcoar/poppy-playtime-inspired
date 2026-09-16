using UnityEngine;
using System.Collections.Generic;

public class AutoSaveManager : MonoBehaviour
{
    public static AutoSaveManager instance { get; private set; }
    
    [SerializeField] private float autoSaveInterval = 300f; // 5 minutes
    [SerializeField] private bool enableAutoSave = true;
    private float timeSinceLastSave = 0f;
    
    [SerializeField] private int maxSaveSlots = 5;
    private Queue<string> saveSlots = new Queue<string>();

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

    private void Update()
    {
        if (!enableAutoSave) return;
        
        timeSinceLastSave += Time.deltaTime;
        
        if (timeSinceLastSave >= autoSaveInterval)
        {
            PerformAutoSave();
            timeSinceLastSave = 0f;
        }
    }

    private void PerformAutoSave()
    {
        SaveSystem.SaveGame();
        Debug.Log("Auto-save completed");
    }

    public void SaveCheckpoint(int checkpointID, string checkpointName)
    {
        SaveSystem.SaveGame();
        Debug.Log($"Checkpoint save: {checkpointName}");
    }

    public void EnableAutoSave(bool enable)
    {
        enableAutoSave = enable;
    }

    public void SetAutoSaveInterval(float interval)
    {
        autoSaveInterval = interval;
    }
}
