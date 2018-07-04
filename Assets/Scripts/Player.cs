using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {
    [SerializeField]
    private Behaviour[] disabledBehavioursOnDeath;
    private bool[] behavioursEnabled;

    [SerializeField]
    private GameObject[] disabledGameObjectsOnDeath;

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private GameObject deathEffectPrefab;

    [SerializeField]
    private GameObject spawnEffectPrefab;

    private bool firstSetup = true;

    public int kills;
    public int deaths;

    [SyncVar]
    private int currentHealth;

    [SyncVar]
    public string username = "Loading...";

    [SyncVar]
    private bool dead = false;
    public bool isDead
    {
        get { return dead; }
        protected set { dead = value; }
    }

    public bool isAlive
    {
        get { return !isDead;  }
        protected set { isDead = !value; }
    }

    public float GetHealthPercent()
    {
        return (float) currentHealth / maxHealth;
    }

    //public void Update()
    //{
    //    if (isLocalPlayer)
    //        if (Input.GetKey("k"))
    //            RpcTakeDamage(10, null);
    //}

    public void PlayerSetup()
    {
        if ( isLocalPlayer )
        {
            GameManager.singleton.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIsingleton.SetActive(true);
            
        }
        CmdBroadCastPlayerSetup();
    }

    [Command]
    private void CmdBroadCastPlayerSetup()
    {
        RpcReceivePlayerSetup();
    }

    [ClientRpc]
    private void RpcReceivePlayerSetup()
    {
        if ( firstSetup )
        {
            behavioursEnabled = new bool[disabledBehavioursOnDeath.Length];
            for (int i = 0; i < behavioursEnabled.Length; i++)
                behavioursEnabled[i] = disabledBehavioursOnDeath[i].enabled;
            firstSetup = false;
        }
        
        setDefautls();
    }

    public void setDefautls()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disabledBehavioursOnDeath.Length; i++)
            disabledBehavioursOnDeath[i].enabled = behavioursEnabled[i];
        for (int i = 0; i < disabledGameObjectsOnDeath.Length; i++)
            disabledGameObjectsOnDeath[i].SetActive(true);
        Collider collider = GetComponent<Collider>();
        if ( collider != null )
            collider.enabled = true;
        Destroy(Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity), 3f);
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage, string sourcePlayerID)
    {
        if ( isAlive )
        {
            currentHealth -= damage;
            if ( currentHealth <= 0 )
                Die(sourcePlayerID);
        }
            
    }

    private void Die(string sourcePlayerID = null)
    {
        isDead = true;
        deaths += 1;
        Player sourcePlayer = GameManager.GetPlayer(sourcePlayerID);
        if (sourcePlayer != null)
        {
            GameManager.singleton.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
            sourcePlayer.kills += 1;
        }
        for (int i = 0; i < disabledBehavioursOnDeath.Length; i++)
            disabledBehavioursOnDeath[i].enabled = false;
        for (int i = 0; i < disabledGameObjectsOnDeath.Length; i++)
            disabledGameObjectsOnDeath[i].SetActive(false);
        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
        Destroy(Instantiate(deathEffectPrefab, transform.position, Quaternion.identity), 3f);
        if ( isLocalPlayer )
        {
            GameManager.singleton.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIsingleton.SetActive(false);
        }
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.singleton.matchSettings.respawnTime);
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(0.1f);
        PlayerWeapon currentWeapon = GetComponent<WeaponManager>().GetCurrentWeapon();
        currentWeapon.bullets = currentWeapon.maxBullets;
        PlayerSetup();
    }
}
