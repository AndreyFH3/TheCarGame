using UnityEngine;

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

    public void SetUnpaused()
    {
        controller.SetUnpaused();
    }

    public void StopRace()
    {
        controller.ExitRace();
    }
}
