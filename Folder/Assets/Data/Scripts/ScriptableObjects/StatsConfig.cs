using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsConfig", menuName = "ScriptableObjects/CreateStatsConfig", order = 2)]
public class StatsConfig : ScriptableObject
{
    [SerializeField] private string upgradeCostsURL;
    [SerializeField] private string carsSettingsURL;
    [SerializeField] private string rewardsURL;
    [SerializeField] private string tracksTimesURL;
    [SerializeField] private string tracksPointsURL;
    [SerializeField] private string carPriceURL;
    [SerializeField] private string mapPriceURL;

    public List<UpgradeCosts> upgradeCosts;
    [SerializeField] private List<CarSettings> carsSettings;
    [SerializeField] private List<Reward> rewards;
    [SerializeField] private List<TrackInfo> times;
    [SerializeField] private List<TrackInfo> points;
    [SerializeField] private List<CarPrice> carPrices;
    [SerializeField] private List<CarPrice> mapPrices;

    [Button]
    private void Download()
    {
        upgradeCosts = FromJson<UpgradeCosts>(GoogleDocsDownloader.Download(upgradeCostsURL, GoogleDocsDownloader.JsonMode.Array));
        carsSettings = FromJson<CarSettings>(GoogleDocsDownloader.Download(carsSettingsURL, GoogleDocsDownloader.JsonMode.Array));
        rewards = FromJson<Reward>(GoogleDocsDownloader.Download(rewardsURL, GoogleDocsDownloader.JsonMode.Array));
        times = FromJson<TrackInfo>(GoogleDocsDownloader.Download(tracksTimesURL, GoogleDocsDownloader.JsonMode.Array));
        points = FromJson<TrackInfo>(GoogleDocsDownloader.Download(tracksPointsURL, GoogleDocsDownloader.JsonMode.Array));
        carPrices = FromJson<CarPrice>(GoogleDocsDownloader.Download(carPriceURL, GoogleDocsDownloader.JsonMode.Array));
        mapPrices = FromJson<CarPrice>(GoogleDocsDownloader.Download(mapPriceURL, GoogleDocsDownloader.JsonMode.Array));
    }

    public CarSettings GetCarSettings(string id) => carsSettings.Find(x => x.CarId == id);

    class Wrapper<T>
    {
        public T[] Items;
    }

    static List<T> FromJson<T>(string json)
    {
        var wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}");
        return wrapper.Items.ToList();
    }
    
    public TrackInfo GetTrackTimes(string id) => times.Find(x => x.Id == id);
    
    public TrackInfo GetTrackPoints(string id) => points.Find(x => x.Id == id);
    
    public Reward GetReward(string id) => rewards.Find(x => x.Id == id);
    
    public int GetCarPrice(string id)
    {
        if (!string.IsNullOrEmpty(id))
            if (carPrices is not null)
            {
                var result = carPrices.Find(x => x.ID == id);
                if (result is not null)
                    return result.PriceOfObject;
            }
        return int.MaxValue;
    }
    public int GetMapPrice(string id)
    {
        if (!string.IsNullOrEmpty(id))
            if (mapPrices is not null)
            {
                var result = mapPrices.Find(x => x.ID == id);
                if (result is not null)
                    return result.PriceOfObject;
            }
        return int.MaxValue;
    }
}

[System.Serializable]
public class UpgradeCosts
{
    [SerializeField] private string CAR_ID;
    [SerializeField] private int LEVEL;
    [SerializeField] private string TYPE;
    [SerializeField] private float PRICE;
    [SerializeField] private float BONUS;

    public string CarId => CAR_ID;
    public int Level => LEVEL;
    public CharacteristicType Type => System.Enum.Parse<CharacteristicType>(TYPE);
    public float Price => PRICE;
    public float Bonus => BONUS;
}

[System.Serializable]
public class CarSettings
{
    [SerializeField] private string Car_ID;
    [SerializeField] private int Max_Speed;
    [SerializeField] private int Max_ReverseSpeed;
    [SerializeField] private int Acceleration_Multiplayer;
    [SerializeField] private int MaxSteering_Angle;
    [SerializeField] private float Steering_Speed;
    [SerializeField] private int Brake_Force;
    [SerializeField] private int Deceleration_Multiplayer;
    [SerializeField] private int Drift_Multiplayer;

    public string CarId => Car_ID;  
    public int MaxSpeed => Max_Speed;
    public int MaxReverseSpeed => Max_ReverseSpeed;
    public int AccelerationMultiplayer => Acceleration_Multiplayer;
    public int MaxSteeringAngle => MaxSteering_Angle;
    public float SteeringSpeed => Steering_Speed;
    public int BrakeForce => Brake_Force;
    public int DecelerationMultiplayer => Deceleration_Multiplayer;
    public int DriftMultiplayer => Drift_Multiplayer;
}

[System.Serializable]
public class Reward
{
    [SerializeField] private string TRACK_ID;
    [SerializeField] private int AMOUNT_1;
    [SerializeField] private int AMOUNT_2;
    [SerializeField] private int AMOUNT_3;
    public string Id => TRACK_ID;
    public int AmountOne => AMOUNT_1;
    public int AmountTwo => AMOUNT_2;
    public int AmountThree => AMOUNT_3;
}

[System.Serializable]
public class TrackInfo

{
    [SerializeField] private string TRACK_ID;
    [SerializeField] private int ONE_STAR;
    [SerializeField] private int TWO_STAR;
    [SerializeField] private int THREE_STAR;
    public string Id => TRACK_ID;
    public int OneStar => ONE_STAR;
    public int TwoStar => TWO_STAR;
    public int ThreeStar => THREE_STAR;

}
[System.Serializable]
public class CarPrice
{
    [SerializeField] private string Id;
    [SerializeField] private int Price;

    public string ID => Id;
    public int PriceOfObject => Price;
}