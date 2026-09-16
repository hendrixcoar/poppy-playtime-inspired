using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private string roomName = "Room";
    [SerializeField] private string roomDescription = "";
    [SerializeField] private int roomID = 0;
    [SerializeField] private Bounds roomBounds;
    [SerializeField] private Light ambientLight;
    [SerializeField] private AudioClip ambientMusic;
    [SerializeField] private float musicVolume = 0.5f;
    
    [Header("Room Features")]
    [SerializeField] private bool hasEnemies = false;
    [SerializeField] private bool hasPuzzles = false;
    [SerializeField] private bool isStartingRoom = false;
    [SerializeField] private Door[] doors;
    
    private bool isPlayerInRoom = false;
    private AudioSource audioSource;

    private void Start()
    {
        if (ambientLight == null)
        {
            ambientLight = GetComponentInChildren<Light>();
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isPlayerInRoom && AudioManager.instance != null && ambientMusic != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = ambientMusic;
                audioSource.volume = musicVolume;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = true;
            OnPlayerEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
            OnPlayerExited();
        }
    }

    private void OnPlayerEntered()
    {
        Debug.Log($"Player entered room: {roomName}");
        
        // Adjust lighting
        if (ambientLight != null)
        {
            ambientLight.enabled = true;
        }
        
        // Spawn enemies if configured
        if (hasEnemies)
        {
            SpawnRoomEnemies();
        }
    }

    private void OnPlayerExited()
    {
        Debug.Log($"Player exited room: {roomName}");
        
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void SpawnRoomEnemies()
    {
        // Enemy spawning logic handled by EnemySpawner
    }

    public void UnlockAllDoors()
    {
        foreach (Door door in doors)
        {
            door.Unlock();
        }
    }

    public void LockAllDoors()
    {
        foreach (Door door in doors)
        {
            door.Lock();
        }
    }

    public string GetRoomName() => roomName;
    public int GetRoomID() => roomID;
    public bool IsPlayerInRoom() => isPlayerInRoom;
    public Door[] GetDoors() => doors;
}
