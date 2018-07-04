using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;


public class JoinGame : MonoBehaviour {
    private NetworkManager networkManager;
    private List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
            networkManager.StartMatchMaker();
        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        status.text = "";
        if (!success || matches == null)
        {
            status.text = "Couldn't get room list";
            return;
        }

        ClearRoomList();

        foreach (MatchInfoSnapshot match in matches)
        {
            GameObject roomListItemInstance = Instantiate(roomListItemPrefab);
            roomListItemInstance.transform.SetParent(roomListParent);

            RoomListItem roomListItem = roomListItemInstance.GetComponent<RoomListItem>();

            if (roomListItem != null)
                roomListItem.Setup(match, JoinRoom);

            roomList.Add(roomListItemInstance);
        }

        if ( roomList.Count == 0 )
            status.text = "No rooms found.";
    }


    private void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
            Destroy(roomList[i]);
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining...";
    }
}
