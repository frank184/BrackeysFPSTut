using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    private WeaponGraphics currentGraphics;

    public bool isReloading = false;

	void Start ()
    {
        EquipWeapon(primaryWeapon);
	}

    public void Reload()
    {
        if (isReloading) return;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        CmdOnReload();
        yield return new WaitForSeconds(currentWeapon.reloadSeconds);
        currentWeapon.bullets = currentWeapon.maxBullets;
        isReloading = false;
    }

    [Command]
    private void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    private void RpcOnReload()
    {
        Animator animator = currentGraphics.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Reload");
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
        weapon.bullets = weapon.maxBullets;
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
