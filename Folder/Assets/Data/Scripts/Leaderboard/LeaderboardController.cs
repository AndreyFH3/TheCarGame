using GamePush;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController : IDisposable
{
    public System.Action<List<LeaderboardFetchData>> OnDataGet;
    private RaceController controller;
    private LeaderboardView view;
    private string mode = "";

    public LeaderboardController(RaceController controller, LeaderboardView view)
    {
        this.controller = controller;
        this.view = view;
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
        GP_Leaderboard.OnFetchError += Error;
        
        GP_Leaderboard.OnLeaderboardOpen += OnOpen;
        GP_Leaderboard.OnLeaderboardClose += OnClose;
        OpenLeaderboard();
        GetLeaderboard();
        view.gameObject.SetActive(false);
    }

    private void OnOpen()
    {
        view.gameObject.SetActive(true);
    }

    private void OnClose()
    {
        view.gameObject.SetActive(false);
    }

    private void Error()
    {
        Debug.Log("Error get Leaderboard");
    }
    
    private void OpenLeaderboard()
    {
        GP_Leaderboard.Open(
            controller.RaceSettings.trackId,
            mode == "_Circle" ? Order.ASC : Order.DESC,
            25,
            10,
            WithMe.last);
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
        GP_Leaderboard.OnFetchError += Error;
        GP_Leaderboard.OnLeaderboardOpen -= OnOpen;
        GP_Leaderboard.OnLeaderboardClose -= OnClose;
    }

    private void SetResult()
    {
        GP_Player.Set(controller.RaceSettings.trackId + mode, controller.RaceTime);

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