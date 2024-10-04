using TMPro;
using UnityEngine;

public class TimerRaceComponent : RaceUIComponent
{
    [SerializeField] private TextMeshProUGUI timerText;
    private RaceController raceController;
    public override void Init(RaceController controller)
    {
        raceController = controller;
    }

    private void Update()
    {
        if (raceController.IsStarted)
        {   
            timerText.text = Localization.Get("RaceTime", raceController.RaceTime.ToString("n2"));
        }
        else
            timerText.text = Localization.Get("RaceTime", 0.ToString("n2"));

    }

}
