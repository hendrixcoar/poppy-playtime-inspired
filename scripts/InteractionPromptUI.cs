using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 5f;
    
    private InteractionDetector interactionDetector;
    private bool isVisible = false;

    private void Start()
    {
        interactionDetector = FindObjectOfType<InteractionDetector>();
        
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (interactionDetector == null) return;
        
        if (interactionDetector.HasInteractable())
        {
            ShowPrompt(interactionDetector.GetInteractionPrompt());
        }
        else
        {
            HidePrompt();
        }
    }

    private void ShowPrompt(string prompt)
    {
        if (promptText != null)
        {
            promptText.text = prompt;
        }
        
        if (canvasGroup != null && !isVisible)
        {
            isVisible = true;
        }
        
        FadeIn();
    }

    private void HidePrompt()
    {
        if (canvasGroup != null && isVisible)
        {
            isVisible = false;
            FadeOut();
        }
    }

    private void FadeIn()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * fadeSpeed);
        }
    }

    private void FadeOut()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime * fadeSpeed);
        }
    }
}
