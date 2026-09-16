using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string objectName = "Object";
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private bool isOneTimeUse = false;
    [SerializeField] private AudioClip interactionSound;
    
    private bool hasBeenUsed = false;
    private Renderer objectRenderer;
    private Color originalColor;
    [SerializeField] private Color highlightColor = Color.yellow;

    public delegate void InteractionDelegate();
    public event InteractionDelegate OnInteracted;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (isOneTimeUse && hasBeenUsed)
        {
            Debug.Log($"{objectName} has already been used");
            return;
        }
        
        Debug.Log($"Interacting with: {objectName}");
        
        if (interactionSound != null && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySound(interactionSound.name);
        }
        
        OnInteracted?.Invoke();
        
        if (isOneTimeUse)
        {
            hasBeenUsed = true;
        }
    }

    public string GetInteractionPrompt()
    {
        if (isOneTimeUse && hasBeenUsed)
        {
            return "Already used";
        }
        return interactionPrompt;
    }

    public bool CanInteract()
    {
        if (isOneTimeUse && hasBeenUsed)
        {
            return false;
        }
        return true;
    }

    public void OnInteractionHighlight(bool isHighlighted)
    {
        if (objectRenderer == null) return;
        
        if (isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
        }
        else
        {
            objectRenderer.material.color = originalColor;
        }
    }
}
