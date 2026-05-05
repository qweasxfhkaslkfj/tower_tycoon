using UnityEngine;
using UnityEngine.Events;

public class UpgradeTower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject upgradeUIPanel; // Reference to the upgrade UI panel
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to press for interaction
    [SerializeField] private float interactionRadius = 2f; // Radius for interaction (visual only)

    [Header("Events")]
    public UnityEvent OnPlayerEnter; // Event triggered when player enters trigger zone
    public UnityEvent OnPlayerExit; // Event triggered when player exits trigger zone
    public UnityEvent OnInteract; // Event triggered when player interacts with tower

    private bool isPlayerInRange = false; // Flag to track if player is inside trigger zone
    private GameObject playerObject; // Reference to the player game object

    void Start()
    {
        // Initially hide the upgrade UI panel when game starts
        if (upgradeUIPanel != null)
            upgradeUIPanel.SetActive(false);
    }

    void Update()
    {
        // Check if player is in range AND presses the interact key
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            OpenUpgradeMenu();
            OnInteract?.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerObject = other.gameObject;
            OnPlayerEnter?.Invoke();

            ShowInteractionPrompt(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerObject = null;
            OnPlayerExit?.Invoke();

            // Automatically close the upgrade menu when player leaves the zone
            if (upgradeUIPanel != null && upgradeUIPanel.activeSelf)
                CloseUpgradeMenu();

            ShowInteractionPrompt(false);
        }
    }

    void OpenUpgradeMenu()
    {
        if (upgradeUIPanel != null)
        {
            upgradeUIPanel.SetActive(true);

            // Update the UI to show current stats and money
            PlayerUpgradeShop shop = upgradeUIPanel.GetComponent<PlayerUpgradeShop>();
            if (shop != null)
                shop.UpdateUI();

            Time.timeScale = 0f;
        }
    }

    public void CloseUpgradeMenu()
    {
        if (upgradeUIPanel != null)
        {
            upgradeUIPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void ShowInteractionPrompt(bool show)
    {
        // Display a debug message as a prompt 
        Debug.Log(show ? $"Press {interactKey} to open upgrade shop" : "");
    }

    void OnDrawGizmosSelected()
    {
        // Draw a green wire sphere to visualize interaction radius in editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}