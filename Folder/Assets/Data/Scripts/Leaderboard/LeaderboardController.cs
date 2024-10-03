using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController: IDisposable
{
    public System.Action<List<LeaderboardFetchData>> OnDataGet;
    private RaceController controller;
    private string mode = "";

    public LeaderboardController(RaceController controller)
    {
        this.controller = controller;
        if (controller is CircleRaceController)
        {
            mode = "_Circle";
        }
        else if (controller is DriftRaceController drift)
        {
            mode = "_Drift";
        }
        SetResult();
        GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;
        GetLeaderboard();
    }

    private void GetLeaderboard()
    {
        GP_Leaderboard.Fetch(
            controller.RaceSettings.trackId + mode,
            controller.RaceSettings.trackId + mode,
            Order.DESC,
            25,
            10,
            WithMe.last);
    }
    
    private void OnFetchSuccess(string fetchTag, GP_Data data)
    {
        var players = data.GetList<LeaderboardFetchData>();
        OnDataGet?.Invoke(players);
    }

    public void Dispose()
    {
        GP_Leaderboard.OnFetchSuccess -= OnFetchSuccess;
    }

    private void SetResult()
    {
        GP_Player.Set(controller.RaceSettings.trackId, controller.RaceTime);

    }
}

public class LeaderboardFetchData
{
    public int id;
    public string name;
    public string avatar;
    public int position;

    public string score;
    public string gold;
    public string level;
}