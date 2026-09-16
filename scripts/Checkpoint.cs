using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string checkpointName = "Checkpoint";
    [SerializeField] private int checkpointID = 0;
    [SerializeField] private bool isAutoCheckpoint = false;
    [SerializeField] private Vector3 respawnOffset = Vector3.zero;
    
    private Vector3 checkpointPosition;
    private Quaternion checkpointRotation;
    private AudioSource checkpointAudio;
    
    public delegate void CheckpointReachedDelegate();
    public event CheckpointReachedDelegate OnCheckpointReached;

    private void Start()
    {
        checkpointPosition = transform.position + respawnOffset;
        checkpointRotation = transform.rotation;
        checkpointAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        Debug.Log($"Checkpoint reached: {checkpointName}");
        
        if (checkpointAudio != null)
        {
            checkpointAudio.Play();
        }
        
        OnCheckpointReached?.Invoke();
        
        // Save checkpoint data
        SaveCheckpointData();
    }

    private void SaveCheckpointData()
    {
        // This would integrate with save/load system
        Debug.Log($"Checkpoint {checkpointID} saved at {checkpointPosition}");
    }

    public Vector3 GetRespawnPosition() => checkpointPosition;
    public Quaternion GetRespawnRotation() => checkpointRotation;
    public string GetCheckpointName() => checkpointName;
    public int GetCheckpointID() => checkpointID;
}
