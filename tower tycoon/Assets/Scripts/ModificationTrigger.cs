using UnityEngine;

public class ModificationTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject modificationUIPanel;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isPlayerInRange = false;
    private GameObject playerObject;


    void Start()
    {
        if (modificationUIPanel != null)
            modificationUIPanel.SetActive(false);

    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            OpenModificationMenu();

            if (interactable != null)
                interactable.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerObject = other.gameObject;
            Debug.Log($"Press {interactKey} to open weapon modification shop");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerObject = null;
            if (modificationUIPanel != null && modificationUIPanel.activeSelf)
                CloseModificationMenu();
        }
    }

    void OpenModificationMenu()
    {
        if (modificationUIPanel != null && playerObject != null)
        {
            modificationUIPanel.SetActive(true);

            WeaponModificationShop shop = modificationUIPanel.GetComponent<WeaponModificationShop>();
            if (shop != null)
            {
                shop.Initialize(playerObject.GetComponent<PlayerStats>());
                shop.UpdateUI();
            }

            Time.timeScale = 0f;
        }
    }

    public void CloseModificationMenu()
    {
        if (modificationUIPanel != null)
        {
            modificationUIPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}