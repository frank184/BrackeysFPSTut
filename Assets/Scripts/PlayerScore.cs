using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

    int lastKills = 0;
    int lastDeaths = 0;

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
            
                SyncNow();
        }
    }

    private void SyncNow()
    {
        UserAccountManager.singleton.LoadData(OnLoadData);
    }

    private void OnLoadData()
    {
        if (player.kills <= lastKills && player.deaths <= lastDeaths) return;
        int killsSinceLast = player.kills - lastKills;
        int deathsSinceLast = player.deaths - lastDeaths;
        if (killsSinceLast == 0 && deathsSinceLast == 0) return;
        UserAccountManager.LoggedInData.kills += killsSinceLast;
        UserAccountManager.LoggedInData.deaths += deathsSinceLast;
        UserAccountManager.singleton.SendData();
        lastKills = player.kills;
        lastDeaths = player.deaths;
    }
}
