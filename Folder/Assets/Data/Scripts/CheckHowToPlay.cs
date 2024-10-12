using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHowToPlay : MonoBehaviour
{
    void Start()
    {
        OpenHowToPlay();
    }

    public void OpenHowToPlay()
    {
        if (Game.Player.IsInited && !Game.Player.isShowedHowTo)
            Instantiate(Game.Config.views.GetHowToPlayView);
    }
}
