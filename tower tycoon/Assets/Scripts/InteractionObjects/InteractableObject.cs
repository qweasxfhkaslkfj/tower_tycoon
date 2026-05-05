using UnityEngine;

public enum UpgradeType 
{ 
    PlayerUpgrade, 
    TurretUpgrade,
    ModifierUpdate
}

public interface IInteractableObject
{
    // Getter
    UpgradeType upgradeType { get; }

    // Stub
    void Interact(PlayerStats playerStats);
}
