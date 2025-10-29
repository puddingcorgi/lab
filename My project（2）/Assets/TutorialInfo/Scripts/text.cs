using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

public class ChargeText : MonoBehaviour
{
    public Text chargeText;

    void Update()
    {
        WeaponManager weapon = FindObjectOfType<WeaponManager>();
        if (weapon == null) return;

        var currentWeapon = weapon.GetType().GetField("currentWeapon", BindingFlags.Public | BindingFlags.Instance).GetValue(weapon);
        var isCharging = (bool)weapon.GetType().GetField("isCharging", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(weapon);
        var chargeTime = (float)weapon.GetType().GetField("grenadeChargeTime", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(weapon);
        var maxCharge = (float)weapon.GetType().GetField("maxGrenadeCharge", BindingFlags.Public | BindingFlags.Instance).GetValue(weapon);

        bool shouldShow = currentWeapon.ToString() == "Grenade" && isCharging;

        if (shouldShow)
        {
            int percent = (int)(chargeTime / maxCharge * 100);
            chargeText.text = $"Charge: {percent}%";
            chargeText.gameObject.SetActive(true);
        }
        else
        {
            chargeText.gameObject.SetActive(false);
        }
    }
}