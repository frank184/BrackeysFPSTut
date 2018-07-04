using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    RectTransform healthFill;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreBoard;

    private Player player;
    private PlayerController controller;
    private WeaponManager weaponManager;

    public void Start()
    {
        PauseMenu.IsOn = false;
    }

    private void Update()
    {
        SetHealthAmout(player.GetHealthPercent());
        SetFuelAmout(controller.GetThrusterFuelAmount());
        SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseMenu();
        if (Input.GetKeyDown(KeyCode.Tab))
            scoreBoard.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            scoreBoard.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }

    public void ToggleScoreBoard()
    {
        scoreBoard.SetActive(!scoreBoard.activeSelf);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    private void SetFuelAmout(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(amount, 1f, 1f);
    }

    private void SetHealthAmout(float amount)
    {
        healthFill.localScale = new Vector3(amount, 1f, 1f);
    }

    private void SetAmmoAmount(int amount)
    {
        ammoText.text = amount.ToString();
    }
}
