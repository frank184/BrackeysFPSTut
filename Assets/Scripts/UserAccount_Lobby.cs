using UnityEngine;
using UnityEngine.UI;

public class UserAccount_Lobby : MonoBehaviour {
    public Text usernameText;
    public Text statsText;

    private void Start()
    {
        if ( UserAccountManager.IsLoggedIn )
        {
            usernameText.text = UserAccountManager.PlayerUsername;
            statsText.text = UserAccountManager.LoggedInData.kills + " kills - " + UserAccountManager.LoggedInData.deaths + " deaths";
        }
    }

    public void LogOut()
    {
        if ( UserAccountManager.IsLoggedIn )
            UserAccountManager.singleton.LogOut();
    }
}
