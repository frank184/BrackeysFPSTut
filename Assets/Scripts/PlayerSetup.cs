using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componenetsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIsingleton;

    public void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            // Disable PlayerGraphics for local player
            Utils.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
            // Instantiate Player UI
            playerUIsingleton = Instantiate(playerUIPrefab);
            playerUIsingleton.name = playerUIPrefab.name;
            // Configure PlayerUI
            PlayerUI ui = playerUIsingleton.GetComponent<PlayerUI>();
            if (ui == null) Debug.Log("No PlayerUI component on Player UI prefab");
            ui.SetController(GetComponent<PlayerController>());
            GetComponent<Player>().PlayerSetup();

            string username = "Loading...";
            if (UserAccountManager.IsLoggedIn)
                username = UserAccountManager.PlayerUsername;
            else username = transform.name;
            CmdSetUsername(transform.name, username);
        }
    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Player player = GameManager.GetPlayer(playerID);
        if (player != null)
            player.username = username;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.AddPlayer(netID, player);
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componenetsToDisable.Length; i++)
            componenetsToDisable[i].enabled = false;
    }

    public void OnDisable()
    {
        Destroy(playerUIsingleton);
        if ( isLocalPlayer ) GameManager.singleton.SetSceneCameraActive(true);
        GameManager.RemovePlayer(transform.name);
    }
}
