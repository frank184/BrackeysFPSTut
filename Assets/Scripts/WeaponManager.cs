using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    private WeaponGraphics currentGraphics;

	void Start ()
    {
        EquipWeapon(primaryWeapon);
	}

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;
        GameObject weaponInstance = Instantiate(
            weapon.graphics, 
            weaponHolder.position, 
            weaponHolder.rotation
        );

        currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.Log("No WeaponGraphics component on the weapon objectL: " + weaponInstance.name);
        }

        weaponInstance.transform.SetParent(weaponHolder);
        if ( isLocalPlayer )
        {
            Utils.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(weaponLayerName)); 
        }
    }


}
