using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    GameObject pauseMenu;

    private PlayerController controller;

    public void Start()
    {
        PauseMenu.IsOn = false;
    }

    private void Update()
    {
        SetFuelAmout(controller.GetThrusterFuelAmount());

        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
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
