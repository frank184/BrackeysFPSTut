using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    [SerializeField]
    private GameObject playerScoreBoardItemPrefab;

    [SerializeField]
    private Transform playerList;

    private void OnEnable()
    {
        Player[] players = GameManager.getPlayers();

        foreach(Player player in players)
        {
            GameObject obj = Instantiate(playerScoreBoardItemPrefab);
            PlayerScoreBoardItem item = obj.GetComponent<PlayerScoreBoardItem>();
            if (item != null)
                item.Setup(player.username, player.kills, player.deaths);
            obj.transform.SetParent(playerList);
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in playerList)
            Destroy(child.gameObject);
    }
}
