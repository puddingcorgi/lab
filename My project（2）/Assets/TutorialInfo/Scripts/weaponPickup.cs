using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponManager.WeaponType weaponType;
    public float respawnTime = 5f;

    [Header("Visual Settings")]
    public Material handgunMaterial;
    public Material machineGunMaterial;
    public Material grenadeMaterial;
    public float rotationSpeed = 50f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;

    private Renderer rend;
    private Vector3 startPosition;
    private bool isActive = true;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startPosition = transform.position;
        SetupAppearance();
    }

    void Update()
    {
        if (isActive)
        {
            
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    void SetupAppearance()
    {
        switch (weaponType)
        {
            case WeaponManager.WeaponType.Handgun:
                rend.material = handgunMaterial;
                break;
            case WeaponManager.WeaponType.MachineGun:
                rend.material = machineGunMaterial;
                break;
            case WeaponManager.WeaponType.Grenade:
                rend.material = grenadeMaterial;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            WeaponManager wm = other.GetComponent<WeaponManager>();
            if (wm != null)
            {
                wm.SwitchWeapon(weaponType);
                Pickup();
            }
        }
    }

    void Pickup()
    {
        isActive = false;
        rend.enabled = false;
        GetComponent<Collider>().enabled = false;

        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        isActive = true;
        rend.enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}