using GamePush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdShowerGarage : MonoBehaviour
{
    public void Start()
    {
        if(AdRewarder.SavedTimeValue == 0)
            AdRewarder.SetLastTimeShoed(Time.time);
        else if (Time.time - AdRewarder.SavedTimeValue > 180)
        {
            ShowAd();
        }
    }

    public void ShowAd()
    {
        GP_Ads.ShowFullscreen();
        AdRewarder.SetLastTimeShoed(Time.time);
    }
}
