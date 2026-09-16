using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Door door;
    [SerializeField] private string interactionPrompt = "Press E to open door";
    [SerializeField] private Renderer doorRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;
    private Color originalColor;

    private void Start()
    {
        if (door == null)
        {
            door = GetComponent<Door>();
        }
        
        if (doorRenderer == null)
        {
            doorRenderer = GetComponent<Renderer>();
        }
        
        if (doorRenderer != null)
        {
            originalColor = doorRenderer.material.color;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (door != null)
        {
            if (door.IsDoorLocked())
            {
                Debug.Log("Door is locked!");
                return;
            }
            
            door.ToggleDoor();
        }
    }

    public string GetInteractionPrompt() => interactionPrompt;

    public bool CanInteract()
    {
        return door != null;
    }

    public void OnInteractionHighlight(bool isHighlighted)
    {
        if (doorRenderer == null) return;
        
        if (isHighlighted)
        {
            doorRenderer.material.color = highlightColor;
        }
        else
        {
            doorRenderer.material.color = originalColor;
        }
    }
}
