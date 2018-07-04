using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DatabaseControl;
public class UserAccountManager : MonoBehaviour {

    public static UserAccountManager singleton;
    public static string PlayerUsername { get; protected set; }
    private static string PlayerPassword = "";

    public static bool IsLoggedIn { get; protected set; }
    public static UserJSON LoggedInData { get; protected set; }


    public string loggedInScene;
    public string loggedOutScene;

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(this);
    }

    public void LogIn(string username, string password)
    {
        PlayerUsername = username;
        PlayerPassword = password;
        IsLoggedIn = true;
        Debug.Log("Logged in as " + username);
        LoadData(() => {
            SceneManager.LoadScene(loggedInScene);
        });
    }

    public void LogOut()
    {
        PlayerUsername = "";
        PlayerPassword = "";
        IsLoggedIn = false;
        LoggedInData = null;
        Debug.Log("Logged out");
        SceneManager.LoadScene(loggedOutScene);
    }

    public delegate void OnLoadData();
    public void LoadData(OnLoadData method = null)
    {
        if ( IsLoggedIn ) StartCoroutine(GetData(method));
    }

    IEnumerator GetData(OnLoadData method = null)
    {
        IEnumerator e = DCF.GetUserData(PlayerUsername, PlayerPassword); // << Send request to get the player's data string. Provides the username and password
        while ( e.MoveNext() ) { yield return e.Current; }
        string response = e.Current as string; // << The returned string from the request
        if ( LoggedInData == null )
            LoggedInData = JsonUtility.FromJson<UserJSON>(response);
        else
            JsonUtility.FromJsonOverwrite(response, LoggedInData);
        Debug.Log("LoggedInData: " + response);
        if (method != null) method();
    }

    public delegate void OnSendData();
    public void SendData(OnSendData method = null)
    {
        if (IsLoggedIn) StartCoroutine(SetData(method));
    }

    IEnumerator SetData(OnSendData method = null)
    {
        IEnumerator e = DCF.SetUserData(PlayerUsername, PlayerPassword, JsonUtility.ToJson(LoggedInData)); // << Send request to set the player's data string. Provides the username, password and new data string
        while ( e.MoveNext() ) { yield return e.Current; }
        string response = e.Current as string; // << The returned string from the request
        JsonUtility.FromJsonOverwrite(response, LoggedInData);
        Debug.Log("SetData: " + response);
        if (method != null) method();
    }
}
