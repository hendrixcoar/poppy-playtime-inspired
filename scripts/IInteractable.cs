using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);
    string GetInteractionPrompt();
    bool CanInteract();
    void OnInteractionHighlight(bool isHighlighted);
}
