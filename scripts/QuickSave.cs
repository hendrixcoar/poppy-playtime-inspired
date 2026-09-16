using UnityEngine;

public class QuickSave : MonoBehaviour
{
    [SerializeField] private KeyCode quickSaveKey = KeyCode.F5;
    [SerializeField] private KeyCode quickLoadKey = KeyCode.F9;
    [SerializeField] private bool enableQuickSave = true;

    private void Update()
    {
        if (!enableQuickSave) return;
        
        if (Input.GetKeyDown(quickSaveKey))
        {
            PerformQuickSave();
        }
        
        if (Input.GetKeyDown(quickLoadKey))
        {
            PerformQuickLoad();
        }
    }

    private void PerformQuickSave()
    {
        SaveSystem.SaveGame();
        Debug.Log("Quick save (F5) completed");
    }

    private void PerformQuickLoad()
    {
        if (SaveSystem.LoadGame())
        {
            Debug.Log("Quick load (F9) completed");
        }
        else
        {
            Debug.LogWarning("Quick load failed - no save data");
        }
    }
}
