using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private RewardView rewardView;
    [SerializeField] private RewardAfterRaceView multiplayer;
    private RaceController raceController;
    public void Init(RaceController controller)
    {
        raceController = controller;
        timeText.text = Localization.Get("RaceTime:", raceController.RaceTime.ToString("n2"));
        winText.text = Localization.Get("win");
        SetReward(controller.GetEarn());
        multiplayer.Init(controller);
        multiplayer.Rewarder.OnChangeValue += SetReward;
    }


    private void SetReward(int value)
    {
        rewardView.SetValues(null, $"+{value}");
    }

    public void GoHome()
    {
        raceController.ExitRace();
    }
}
