using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());

    }

    private void OnDestroy()
    {
        if (player != null)
            SyncNow();
    }

    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (player.kills != 0 || player.deaths != 0)
                SyncNow();
        }
    }

    private void SyncNow()
    {
        UserAccountManager.singleton.LoadData(OnLoadData);
    }

    private void OnLoadData()
    {
        UserAccountManager.LoggedInData.kills += player.kills;
        UserAccountManager.LoggedInData.deaths += player.deaths;
        UserAccountManager.singleton.SendData();
        player.kills = 0;
        player.deaths = 0;
    }
}
