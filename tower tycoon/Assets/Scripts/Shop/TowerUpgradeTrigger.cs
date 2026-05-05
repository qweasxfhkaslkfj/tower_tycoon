using UnityEngine;
using UnityEngine.Events;

public class UpgradeTower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject upgradeUIPanel;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactionRadius = 2f;

    [Header("Events")]
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public UnityEvent OnInteract;

    private bool isPlayerInRange = false;
    private GameObject playerObject;
    private PlayerUpgradeShopLogic shopLogic;

    void Start()
    {
        if (upgradeUIPanel != null)
            upgradeUIPanel.SetActive(false);

        shopLogic = GetComponent<PlayerUpgradeShopLogic>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            OpenUpgradeMenu();
            OnInteract?.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerObject = null;
            OnPlayerExit?.Invoke();

            if (upgradeUIPanel != null && upgradeUIPanel.activeSelf)
                CloseUpgradeMenu();

            ShowInteractionPrompt(false);
        }
    }

    void OpenUpgradeMenu()
    {
        PlayerStats stats = playerObject?.GetComponent<PlayerStats>();

        if (stats != null)
        {
            if (shopLogic != null)
            {
                shopLogic.Interact(stats);
            }
            else
            {
                if (upgradeUIPanel != null)
                {
                    upgradeUIPanel.SetActive(true);
                    PlayerUpgradeShopUI shopUI = upgradeUIPanel.GetComponent<PlayerUpgradeShopUI>();
                    if (shopUI != null)
                        shopUI.SetPlayerStats(stats);
                    Time.timeScale = 0f;
                }
            }
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
        Debug.Log(show ? $"Press {interactKey} to open upgrade shop" : "");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}