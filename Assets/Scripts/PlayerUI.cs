using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreBoard;

    private PlayerController controller;

    public void Start()
    {
        PauseMenu.IsOn = false;
    }

    private void Update()
    {
        SetFuelAmout(controller.GetThrusterFuelAmount());
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

    public void SetController(PlayerController controller)
    {
        this.controller = controller;
    }

    private void SetFuelAmout(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(amount, 1f, 1f);
    }
}
