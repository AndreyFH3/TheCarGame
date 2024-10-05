using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private LeaderboardPlayerView template;
    [SerializeField] private RectTransform parentCards;
    
    private LeaderboardController leaderboard;
    private RaceController controller;
    private List<LeaderboardPlayerView> cards = new List<LeaderboardPlayerView>();

    public void Init(RaceController controller)
    {
        this.controller = controller;
        template.gameObject.SetActive(false);
        leaderboard = new LeaderboardController(controller, this);
        leaderboard.OnDataGet += CreateLeaders;
    }

    private void CreateLeaders(List<LeaderboardFetchData> players)
    {
        cards.ForEach(p => { Destroy(p.gameObject); });
        cards.Clear();

        foreach (var player in players)
        {
            var instance = Instantiate(template, parentCards);
            instance.Init(player);
            instance.gameObject.SetActive(true);
            cards.Add(instance);
        }
    }

    private void OnDestroy()
    {
        leaderboard.OnDataGet -= CreateLeaders;
        leaderboard.Dispose();
    }
}
