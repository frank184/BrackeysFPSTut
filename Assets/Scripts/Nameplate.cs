using UnityEngine;
using UnityEngine.UI;

public class Nameplate : MonoBehaviour {
    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private RectTransform healthFill;


    [SerializeField]
    private Player player;

    private void Update()
    {
        usernameText.text = player.username;
        healthFill.localScale = new Vector3(player.GetHealthPercent(), 1f, 1f);
    }

}
