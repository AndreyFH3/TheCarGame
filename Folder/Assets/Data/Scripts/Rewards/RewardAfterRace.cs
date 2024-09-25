using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RewardAfterRace
{
    [SerializeField] private List<SliderValue> multiplayers;
    public System.Action<int> OnChangeValue;

    public void GiveReward(int value, float multiplayer)
    {
        var sorted = multiplayers.OrderBy(x => x.Value).ToList();
        float multiplayerFinal = 0;
        foreach (var multy in sorted)
        {
            if(multy.Value >= multiplayer)
            {
                multiplayerFinal = multy.Multiplayer;
                break;
            }
        }
        var result = AdRewarder.CalculateResult(value, multiplayerFinal);
        Game.Player.wallet.EarnSoft(result - value);
        OnChangeValue?.Invoke(result);
    }
    [System.Serializable]   
    
    private class SliderValue
    {
        [SerializeField] private float value;
        [SerializeField] private float multiplayer;

        public float Value => value;    
        public float Multiplayer => multiplayer;    
    }
}


