using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private RewardView rewardView;
    private RaceController raceController;
    public void Init(RaceController controller)
    {
        raceController = controller;
        timeText.text = $"RaceTime: {raceController.RaceTime.ToString("n2")} sec";
        winText.text = "win";
        rewardView.SetValues(null, $"+{controller.GetEarn()}");
    }


    public void GoHome()
    {
        raceController.ExitRace();
    }
}
