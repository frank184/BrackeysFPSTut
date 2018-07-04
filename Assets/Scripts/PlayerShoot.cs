using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private static string PLAYER_TAG = "Player";

    private PlayerWeapon currentWeapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;

    public void Start()
    {
        if (cam == null)
        {
            Debug.Log("PlayerShoot requires a camera reference");
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
    }

    public void Update()
    {
        if (PauseMenu.IsOn) return;

        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
                Shoot();
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else
            {
                if ( Input.GetButtonUp("Fire1") )
                    CancelInvoke("Shoot");
            }
        }
        
    }

    [Client]
    private void Shoot()
    {

        if ( isLocalPlayer )
        {
            // we are shooting call server
            CmdOnShoot();

            RaycastHit hit; // filled by Physics.Raycast
            bool physicsRaycast = Physics.Raycast(
                cam.transform.position,
                cam.transform.forward,
                out hit,
                currentWeapon.range,
                mask
            );
            if (physicsRaycast)
            {
                if (hit.collider.tag == PLAYER_TAG)
                    CmdPlayerShot(hit.collider.name, currentWeapon.damage, transform.name);
                CmdOnHit(hit.point, hit.normal);
            }
                
        }
        
        
    }

    [Command]
    private void CmdPlayerShot(string playerID, int damage, string sourcePlayerId)
    {
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage, sourcePlayerId);
    }

    [Command] // server calls rpc for all clients
    private void CmdOnShoot()
    {
        RpcShootEffect();
    }

    [ClientRpc] // sent to all clients
    private void RpcShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Command]
    private void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcHitEffect(pos, normal);
    }

    [ClientRpc] // sent to all clients
    private void RpcHitEffect(Vector3 pos, Vector3 normal)
    {
        // Look into Object Pooling
        GameObject prefab = weaponManager.GetCurrentGraphics().hitEffectPrefab;
        GameObject hitEffect = Instantiate(prefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 1f);
    }

    
}
