// WeaponManager.cs
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType { Handgun, MachineGun, Grenade }
    public WeaponType currentWeapon = WeaponType.Handgun;

    [Header("Weapon References")]
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public Transform firePoint;

    [Header("Weapon Settings")]
    public float handgunFireRate = 0.5f;
    public float machineGunFireRate = 0.1f;
    public float maxGrenadeCharge = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 500f;
    public float machineGunSpread = 0.1f;

    private float handgunTimer = 0f;
    private float machineGunTimer = 0f;
    private float grenadeChargeTime = 0f;
    private bool isCharging = false;

    void Update()
    {
        HandleWeaponInput();
        UpdateTimers();
    }

    void HandleWeaponInput()
    {
        switch (currentWeapon)
        {
            case WeaponType.Handgun:
                if (Input.GetMouseButtonDown(0) && handgunTimer <= 0)
                {
                    FireHandgun();
                    handgunTimer = handgunFireRate;
                }
                break;

            case WeaponType.MachineGun:
                if (Input.GetMouseButton(0) && machineGunTimer <= 0)
                {
                    FireMachineGun();
                    machineGunTimer = machineGunFireRate;
                }
                break;

            case WeaponType.Grenade:
                if (Input.GetMouseButtonDown(0))
                {
                    StartCharging();
                }
                if (Input.GetMouseButtonUp(0) && isCharging)
                {
                    ThrowGrenade();
                }
                break;
        }
    }

    void UpdateTimers()
    {
        if (handgunTimer > 0) handgunTimer -= Time.deltaTime;
        if (machineGunTimer > 0) machineGunTimer -= Time.deltaTime;

        if (isCharging)
        {
            grenadeChargeTime += Time.deltaTime;
            grenadeChargeTime = Mathf.Min(grenadeChargeTime, maxGrenadeCharge);
        }
    }

    void FireHandgun()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void FireMachineGun()
    {
        Vector3 spread = new Vector3(
            Random.Range(-machineGunSpread/10, machineGunSpread/10),
            Random.Range(-machineGunSpread/10, machineGunSpread / 10),
            Random.Range(-machineGunSpread / 10, machineGunSpread / 10)
        );

        Quaternion randomRotation = firePoint.rotation * Quaternion.Euler(spread * 100f);
        Instantiate(bulletPrefab, firePoint.position, randomRotation);
    }

    void StartCharging()
    {
        isCharging = true;
        grenadeChargeTime = 0f;
    }

    void ThrowGrenade()
    {
        float charge = grenadeChargeTime / maxGrenadeCharge;
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        float throwForce = Mathf.Lerp(10f, 25f, charge);
        rb.AddForce(firePoint.forward * throwForce, ForceMode.Impulse);

        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        grenadeScript.explosionForce = explosionForce;
        grenadeScript.explosionRadius = explosionRadius;

        isCharging = false;
        grenadeChargeTime = 0f;
    }

    public void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
    }
}