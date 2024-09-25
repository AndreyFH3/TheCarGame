using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIComponent : RaceUIComponent
{
    [SerializeField] private Transform showPart;
    private RaceController controller;

    public override void Init(RaceController controller)
    {
        this.controller = controller;
    }

    private void Update()
    {
        showPart.gameObject.SetActive(controller.Pause);
    }

    public void RestartGame()
    {
        Race.SetSettings(controller.RaceSettings);
        Time.timeScale = 1;
        SceneManager.LoadScene(2);    
    }

    public void SetUnpaused()
    {
        controller.SetUnpaused();
    }

    public void StopRace()
    {
        controller.ExitRace();
    }
}
