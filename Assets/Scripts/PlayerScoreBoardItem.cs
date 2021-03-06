﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBoardItem : MonoBehaviour {
    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private Text killsText;

    [SerializeField]
    private Text deathsText;

    public void Setup(string username, int kills, int deaths)
    {
        usernameText.text = username;
        killsText.text = "Kills: " + kills;
        deathsText.text = "Deaths: " + deaths;
    }
}
