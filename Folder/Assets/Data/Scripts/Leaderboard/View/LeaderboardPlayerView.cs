using TMPro;
using UnityEngine;

public class LeaderboardPlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI place;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI value;

    public void Init(LeaderboardFetchData playerData)
    {
        place.text = playerData.position.ToString();
        playerName.text = playerData.name;
        value.text = playerData.score.ToString();
    }
}
