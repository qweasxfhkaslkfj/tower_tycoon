using UnityEngine;

public class ModificationTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject modificationUIPanel;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isPlayerInRange = false;

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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Press {interactKey} to open weapon modification shop");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (modificationUIPanel != null && modificationUIPanel.activeSelf)
                CloseModificationMenu();
        }
    }

    void OpenModificationMenu()
    {
        if (modificationUIPanel != null)
        {
            modificationUIPanel.SetActive(true);

            // Update UI with current data
            WeaponModificationShop shop = modificationUIPanel.GetComponent<WeaponModificationShop>();
            if (shop != null)
                shop.UpdateUI();

            // Pause game while shop is open
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