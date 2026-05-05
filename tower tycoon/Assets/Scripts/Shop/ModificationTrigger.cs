using UnityEngine;

public class ModificationTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject modificationUIPanel;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isPlayerInRange = false;
    private WeaponModificationShopLogic shopLogic;

    void Start()
    {
        if (modificationUIPanel != null)
            modificationUIPanel.SetActive(false);

        shopLogic = GetComponent<WeaponModificationShopLogic>();
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = player?.GetComponent<PlayerStats>();

        if (stats != null)
        {
            if (shopLogic != null)
            {
                shopLogic.Interact(stats);
            }
            else
            {
                if (modificationUIPanel != null)
                {
                    modificationUIPanel.SetActive(true);
                    WeaponModificationShopUI shopUI = modificationUIPanel.GetComponent<WeaponModificationShopUI>();
                    if (shopUI != null)
                        shopUI.SetPlayerStats(stats);
                    Time.timeScale = 0f;
                }
            }
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