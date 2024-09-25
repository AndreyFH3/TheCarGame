using GamePush;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardAfterRaceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI multiplayer;
    [SerializeField] private Slider slider;

    [SerializeField] private RewardAfterRace rewarder;
    [SerializeField, Range(0,5)] private float speed = .1f;
    private float selectedValue = 0;
    private bool isBack = false;
    private RaceController controller;

    public RewardAfterRace Rewarder => rewarder;

    public void Init(RaceController controller)
    {
        this.controller = controller;
    }

    private void Update()
    {
        var multipl = isBack ? -1 : 1;

        slider.value += multipl * Time.deltaTime * speed;
        if(slider.value >= 1)
        {
            isBack = true;
        }
        else if(slider.value <= 0)
        {
            isBack = false;
        }
    }

    public void GetReward()
    {
        GP_Ads.ShowRewarded("",
            stringData => 
            { 
                rewarder.GiveReward(controller.GetEarn(), slider.value);
            },
            () => 
            {
                gameObject.SetActive(false);
            },
            isTrue => { });
    }

}
