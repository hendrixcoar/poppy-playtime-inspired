using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int levelNumber;
    public float gameTime;
    public int playerHealth;
    public int sanityLevel;
    public Vector3 playerPosition;
    public List<ItemData> inventoryItems;
    public List<CheckpointData> checkpoints;
    public Dictionary<string, bool> puzzleCompletionStatus;
    public int currentCheckpointID;

    public GameData()
    {
        levelNumber = 1;
        gameTime = 0f;
        playerHealth = 100;
        sanityLevel = 100;
        playerPosition = Vector3.zero;
        inventoryItems = new List<ItemData>();
        checkpoints = new List<CheckpointData>();
        puzzleCompletionStatus = new Dictionary<string, bool>();
        currentCheckpointID = -1;
    }
}

[System.Serializable]
public class ItemData
{
    public string itemID;
    public string itemName;
    public int quantity;

    public ItemData(string id, string name, int qty)
    {
        itemID = id;
        itemName = name;
        quantity = qty;
    }
}

[System.Serializable]
public class CheckpointData
{
    public int checkpointID;
    public string checkpointName;
    public Vector3 position;
    public Quaternion rotation;

    public CheckpointData(int id, string name, Vector3 pos, Quaternion rot)
    {
        checkpointID = id;
        checkpointName = name;
        position = pos;
        rotation = rot;
    }
}
