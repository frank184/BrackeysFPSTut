using UnityEngine;
using UnityEngine.UI;

public class KillfeedItem : MonoBehaviour {
    [SerializeField]
    private Text feedText;

    public int destroyAfter = 10;

    public void Setup(string murderer, string victim)
    {
        feedText.text = "<b><color=#FF4040FF>" + murderer + "</color></b> killed <b><color=#527FFFFF>" + victim + "</color></b>";
    }
}
