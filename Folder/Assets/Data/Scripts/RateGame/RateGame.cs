using GamePush;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RateGame
{
    [SerializeField] private bool isRated = false;
    
    public bool IsRated => isRated;

    public void Init()
    {
        if(!isRated)
            SceneManager.sceneUnloaded += ShowReward;
    }

    public void ShowReward(Scene scene)
    {
        if (scene.name.Contains("Track"))
        {
            var instance = GameObject.Instantiate(Game.Config.views.GetRateWindow);
            instance.Init(this);
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

    }
}