using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // Public fields
    private InteractableObject currentInteractable;
    public GameObject promptPanel;
    public Text promptText;

    // Start Method
    private void Start()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);

        if (GameInput.Instance != null)
            GameInput.Instance.OnInteract += OnInteractPressed;
    }

    // Pressing a key
    private void OnInteractPressed()
    {
        if (currentInteractable != null)
            currentInteractable.Interact();
    }

    // Object destruction
    private void OnDestroy()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnInteract -= OnInteractPressed;
    }

    // Checking the location on the object
    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            ShowPrompt(true);
        }
    }

    // Show the hint
    private void ShowPrompt(bool show)
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(show);
            if (show && promptText != null)
                promptText.text = "Нажмите E для улучшения";
        }
    }

    // Checking the exit at the object
    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();

        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
            ShowPrompt(false);
        }
    }
}
