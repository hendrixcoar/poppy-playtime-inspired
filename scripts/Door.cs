using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string doorName = "Door";
    [SerializeField] private Room connectedRoom;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private string requiredKeyID = "";
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float openAngle = 90f;
    
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isDoorOpen = false;
    private Animator doorAnimator;
    private AudioSource doorAudioSource;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        
        doorAnimator = GetComponent<Animator>();
        doorAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            AttemptToDoor();
        }
    }

    private void AttemptToDoor()
    {
        if (isLocked)
        {
            if (requiresKey)
            {
                if (HasRequiredKey())
                {
                    Unlock();
                    OpenDoor();
                }
                else
                {
                    Debug.Log($"Door locked! Requires: {requiredKeyID}");
                }
            }
            return;
        }
        
        ToggleDoor();
    }

    private bool HasRequiredKey()
    {
        if (Inventory.instance == null)
            return false;
        
        // Check if player has the required key item
        // This would require a key item in the inventory
        return true; // Placeholder
    }

    public void OpenDoor()
    {
        if (isDoorOpen) return;
        
        isDoorOpen = true;
        
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", true);
        }
        else
        {
            StartCoroutine(RotateDoor(openRotation));
        }
        
        if (doorAudioSource != null)
        {
            doorAudioSource.Play();
        }
        
        Debug.Log($"Opened: {doorName}");
    }

    public void CloseDoor()
    {
        if (!isDoorOpen) return;
        
        isDoorOpen = false;
        
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", false);
        }
        else
        {
            StartCoroutine(RotateDoor(closedRotation));
        }
        
        Debug.Log($"Closed: {doorName}");
    }

    public void ToggleDoor()
    {
        if (isDoorOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    public void Lock()
    {
        isLocked = true;
        CloseDoor();
    }

    public void Unlock()
    {
        isLocked = false;
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
    }

    public bool IsDoorOpen() => isDoorOpen;
    public bool IsDoorLocked() => isLocked;
    public string GetDoorName() => doorName;
}
