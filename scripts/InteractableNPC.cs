using UnityEngine;

public class InteractableNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "NPC";
    [SerializeField] private string interactionPrompt = "Press E to talk";
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private AudioClip dialogueAudio;
    
    private bool isInDialogue = false;
    private Renderer npcRenderer;
    private Color originalColor;
    [SerializeField] private Color highlightColor = Color.cyan;
    private int currentDialogueLine = 0;

    private void Start()
    {
        npcRenderer = GetComponent<Renderer>();
        if (npcRenderer != null)
        {
            originalColor = npcRenderer.material.color;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (isInDialogue) return;
        
        StartDialogue();
    }

    private void StartDialogue()
    {
        isInDialogue = true;
        currentDialogueLine = 0;
        ShowDialogueLine();
    }

    private void ShowDialogueLine()
    {
        if (currentDialogueLine < dialogueLines.Length)
        {
            Debug.Log($"{npcName}: {dialogueLines[currentDialogueLine]}");
            currentDialogueLine++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isInDialogue = false;
        currentDialogueLine = 0;
        Debug.Log($"{npcName} dialogue ended");
    }

    public string GetInteractionPrompt()
    {
        return isInDialogue ? "(In dialogue)" : interactionPrompt;
    }

    public bool CanInteract()
    {
        return !isInDialogue;
    }

    public void OnInteractionHighlight(bool isHighlighted)
    {
        if (npcRenderer == null) return;
        
        if (isHighlighted)
        {
            npcRenderer.material.color = highlightColor;
        }
        else
        {
            npcRenderer.material.color = originalColor;
        }
    }
}
