using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Transform interactionSource;
    
    private IInteractable currentInteractable;
    private RaycastHit lastHit;

    private void Start()
    {
        if (interactionSource == null)
        {
            interactionSource = transform;
        }
    }

    private void Update()
    {
        DetectInteractables();
        HandleInteractionInput();
    }

    private void DetectInteractables()
    {
        IInteractable newInteractable = null;
        
        // Raycast from camera/player
        if (Physics.Raycast(interactionSource.position, interactionSource.forward, out RaycastHit hit, interactionRange, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                newInteractable = interactable;
                lastHit = hit;
            }
        }
        
        // Handle interaction change
        if (newInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnInteractionHighlight(false);
            }
            
            currentInteractable = newInteractable;
            
            if (currentInteractable != null)
            {
                currentInteractable.OnInteractionHighlight(true);
                Debug.Log($"Interactable detected: {currentInteractable.GetInteractionPrompt()}");
            }
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            currentInteractable.Interact(gameObject);
        }
    }

    public IInteractable GetCurrentInteractable() => currentInteractable;
    public string GetInteractionPrompt() => currentInteractable?.GetInteractionPrompt() ?? "";
    public bool HasInteractable() => currentInteractable != null;
}
