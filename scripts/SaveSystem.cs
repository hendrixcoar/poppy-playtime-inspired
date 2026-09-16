using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private static string savePath;
    private static string saveFileName = "gamesave.json";

    static SaveSystem()
    {
        savePath = Application.persistentDataPath;
    }

    public static void SaveGame()
    {
        GameData gameData = GatherGameData();
        
        try
        {
            string json = JsonUtility.ToJson(gameData, true);
            string fullPath = Path.Combine(savePath, saveFileName);
            
            File.WriteAllText(fullPath, json);
            Debug.Log($"Game saved successfully at: {fullPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public static bool LoadGame()
    {
        try
        {
            string fullPath = Path.Combine(savePath, saveFileName);
            
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"Save file not found at: {fullPath}");
                return false;
            }
            
            string json = File.ReadAllText(fullPath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            
            ApplyGameData(gameData);
            Debug.Log("Game loaded successfully");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return false;
        }
    }

    public static bool HasSaveData()
    {
        string fullPath = Path.Combine(savePath, saveFileName);
        return File.Exists(fullPath);
    }

    public static void DeleteSaveData()
    {
        try
        {
            string fullPath = Path.Combine(savePath, saveFileName);
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Debug.Log("Save data deleted successfully");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to delete save data: {e.Message}");
        }
    }

    private static GameData GatherGameData()
    {
        GameData data = new GameData();
        
        // Gather game state
        if (GameManager.instance != null)
        {
            data.levelNumber = GameManager.instance.GetCurrentLevel();
            data.playerHealth = GameManager.instance.GetPlayerHealth();
            data.sanityLevel = GameManager.instance.GetSanityLevel();
        }
        
        // Gather player position
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            data.playerPosition = player.transform.position;
        }
        
        // Gather inventory
        if (Inventory.instance != null)
        {
            var slots = Inventory.instance.GetSlots();
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty())
                {
                    data.inventoryItems.Add(
                        new ItemData(slot.item.itemID, slot.item.itemName, slot.quantity)
                    );
                }
            }
        }
        
        data.gameTime = Time.time;
        return data;
    }

    private static void ApplyGameData(GameData data)
    {
        // Apply game state
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadLevel(data.levelNumber);
            
            // Restore health and sanity
            int healthDiff = 100 - data.playerHealth;
            if (healthDiff > 0)
            {
                GameManager.instance.DamagePlayer(healthDiff);
            }
        }
        
        // Restore player position
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = data.playerPosition;
        }
        
        // Restore inventory
        if (Inventory.instance != null)
        {
            Inventory.instance.ClearInventory();
            
            // This requires Item references - simplified for now
            Debug.Log($"Inventory items to restore: {data.inventoryItems.Count}");
        }
    }

    public static string GetSaveInfo()
    {
        if (!HasSaveData())
            return "No save data found";
        
        try
        {
            string fullPath = Path.Combine(savePath, saveFileName);
            string json = File.ReadAllText(fullPath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            
            return $"Level {data.levelNumber} - Health: {data.playerHealth} - Sanity: {data.sanityLevel}";
        }
        catch
        {
            return "Error reading save data";
        }
    }
}
