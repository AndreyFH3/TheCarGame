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
    [SerializeField] private LeaderboardView leadrboardView;

    private RaceController raceController;
    public void Init(RaceController controller)
    {
        raceController = controller;
        if(controller is CircleRaceController)
            timeText.text = Localization.Get("RaceTime", raceController.RaceTime.ToString("n2"));
        else if (controller is DriftRaceController drift)
            timeText.text = $"{Localization.Get("Points")}: {Mathf.RoundToInt(drift.DriftPoints)}";

        var reward = controller.GetEarn();
        winText.text = reward == 0 ? Localization.Get("lose") : Localization.Get("win");
        SetReward(reward);
        if(reward == 0)
        {
            multiplayer.gameObject.SetActive(false);
        }
        else
        {
            multiplayer.Init(controller);
            multiplayer.Rewarder.OnChangeValue += SetReward;
        }
        leadrboardView.Init(controller);

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
