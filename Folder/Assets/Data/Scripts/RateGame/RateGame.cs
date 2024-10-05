using GamePush;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RateGame
{
    [SerializeField] private bool isRated = false;
    
    public bool IsRated => isRated;
    private float lastTimeShow = 180;
    public void Init()
    {
        if (!isRated)
        {
            SceneManager.sceneUnloaded += ShowReward;
        }
    }

    public void ShowReward(Scene scene)
    {
        if (!isRated && scene.name.Contains("Track") && lastTimeShow - Time.time < 0)
        {
            var instance = GameObject.Instantiate(Game.Config.views.GetRateWindow);
            instance.Init(this);
            lastTimeShow = 180 + Time.time;
        }
    }

    public void ShowRate()
    {
        GP_App.ReviewRequest(OnReviewResult, OnReviewClose);
    }

    public void OnReviewResult(int value)//success
    {
        Game.Player.wallet.EarnSoft(Game.Config.RewardForRate);
        if (!isRated)
        {
            SceneManager.sceneUnloaded += ShowReward;
        }
        isRated = true;
    }

    public void OnReviewClose(string info)//error
    {
        isRated = true;
    }
}