using UnityEngine;

public class WeaponDiscountManager : MonoBehaviour
{
    private static WeaponDiscountManager instance;
    public static WeaponDiscountManager Instance => instance;

    private float currentDiscount = 0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetDiscount(float discount)
    {
        currentDiscount = discount;
        Debug.Log($"Weapon upgrade discount: {currentDiscount * 100}%");
    }

    public float GetDiscountedPrice(float originalPrice)
    {
        return originalPrice * (1f - currentDiscount);
    }

    public float GetCurrentDiscount() => currentDiscount;
}