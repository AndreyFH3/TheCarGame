using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateWindow : MonoBehaviour
{
    private RateGame rate;

    public System.Action onDestroy;

    public void Init(RateGame rate)
    {
        this.rate = rate;
    }

    public void RateGame()
    {
        rate.ShowRate();
    }

    public void Close()
    {
        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}
