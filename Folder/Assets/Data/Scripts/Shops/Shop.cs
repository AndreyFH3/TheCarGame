using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarShop : Shop
{
    [SerializeField] public List<string> carIds = new List<string>();
    
    private Wallet wallet => Game.Player.wallet;

    public override bool TryBuy(string carId)
    {
        var price = Game.Config.statsConfig.GetCarPrice(carId);
        if (wallet.SpendSoft(price))
        {
            carIds.Add(carId);
            return true;
        }
        return false;
    }

    public override bool Check(string carId)
    {
        var res = carIds.Find(x => x == carId);
        return res is not null;
    }
}

[System.Serializable]
public class MapShop : Shop
{
    [SerializeField] public List<string> mapsIds = new List<string>();

    private Wallet wallet => Game.Player.wallet;

    public override bool TryBuy(string mapId)
    {
        var price = Game.Config.statsConfig.GetMapPrice(mapId);
        if (wallet.SpendSoft(price))
        {
            mapsIds.Add(mapId);
            return true;
        }
        return false;
    }

    public override bool Check(string carId)
    {
        var res = mapsIds.Find(x => x == carId);
        return res is not null;
    }
}

public abstract class Shop
{
    public abstract bool TryBuy(string id);

    public abstract bool Check(string carId);

}