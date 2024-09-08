using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUIComponent : RaceUIComponent
{
    [SerializeField] private Button pauseButton;
    private RaceController controller;
    
    public override void Init(RaceController controller)
    {
        this.controller = controller;
    }

    public void SetPaused()
    {
        controller.SetPaused();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (controller.Pause)
                controller.SetUnpaused();
            else 
                controller.SetPaused();
        }
    }
}
