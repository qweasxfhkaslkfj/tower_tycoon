using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Public fields
    public UpgradeType upgradeType;
    public enum UpgradeType 
    { 
        PlayerUpgrade, 
        TurretUpgrade,
        ModifierUpdate
    }

    // Stub
    public void Interact()
    {
        Debug.Log($"Взаимодействие с {upgradeType}");
    }
}
