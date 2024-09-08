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
        timerText.text = $"RaceTime: {raceController.RaceTime.ToString("n2")} sec";
    }

}
