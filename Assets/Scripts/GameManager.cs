using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;

    public void Awake()
    {
        if (singleton == null) singleton = this;
        else Debug.LogError("Multiple GameManagers detected!");
    }

    #region SceneCamera
    [SerializeField]
    private GameObject sceneCamera;

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera != null)
            sceneCamera.SetActive(isActive);
    }
    #endregion

    #region Player Dictionary
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private const string PLAYER_PREFIX = "Player ";

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    var enumerator = players.GetEnumerator();
    //    while ( enumerator.MoveNext() )
    //    {
    //        string playerID = enumerator.Current.Key;
    //        Player player = enumerator.Current.Value;
    //        GUILayout.Label(playerID + " - " + player.transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    public static void AddPlayer(string netID, Player player)
    {
        player.transform.name = PLAYER_PREFIX + netID;
        players.Add(player.transform.name, player);
    }

    public static void RemovePlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }
    #endregion

    #region Matchmaking
    public MatchSettings matchSettings;

    
    #endregion
}
