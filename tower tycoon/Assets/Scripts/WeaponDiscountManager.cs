using UnityEngine;

public class WeaponUpgradeManager : MonoBehaviour
{
    private static WeaponUpgradeManager instance;
    public static WeaponUpgradeManager Instance => instance;

    private float currentDiscount = 0f; // Current discount percentage (0 = no discount, 0.5 = 50% off)

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Sets the current discount value and logs it
    public void SetDiscount(float discount)
    {
        currentDiscount = discount;
        Debug.Log($"Weapon upgrade discount: {currentDiscount * 100}%");
    }

    // Calculates the discounted price based on current discount
    public float GetDiscountedPrice(float originalPrice)
    {
        return originalPrice * (1f - currentDiscount);
    }

    // Public getter to retrieve current discount value
    public float GetCurrentDiscount() => currentDiscount;
}