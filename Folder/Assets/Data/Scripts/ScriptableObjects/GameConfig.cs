using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/CreateGameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private List<Car> cars;
    public List<TracksList> tracksList;
    public List<SkyBoxMaterial> skyBoxes;
    public Icons icons;
    public Views views;
    [SerializeField] private List<RaceUI> raceUI;
    [SerializeField] private List<TrackPart> trackParts;
    [SerializeField] private TrackWay colliderExample;
    [SerializeField] private int rewardForRate = 5000;
    [SerializeField] private int adReward = 5000;

    public StatsConfig statsConfig;

    public int RewardForRate => rewardForRate;
    public int AdReward => adReward;
    public RaceUIComponent[] GetRaceUICompnents(RaceType id) => raceUI.Find(x => x.Id == id).Components;

    public TrackWay ColliderExample => colliderExample;
    public Car[] GetCars => cars.ToArray();

    public TrackPart GetTrackPart(string id) => trackParts.Find(x => x.Id == id);

    public TracksList GetTrack(string id) => tracksList.Find(x => x.Id == id);
    public List<string> GetTrackIds() => tracksList.Select(x => x.Id).ToList();
    public Material GetSkyBox(bool isDay)
    {
        if (isDay) return skyBoxes.Find(x => x.Id == "Day").SkyBox;
        return skyBoxes.Find(x => x.Id == "Night").SkyBox;
    }

    public Car GetCar(string id) => cars.Find(x => x.Id == id);
    public List<string> GetCarsId()
    {
        return cars.Select(x => x.Id).ToList();
    }



    [System.Serializable]    
    public class SkyBoxMaterial
    {
        [SerializeField] private string id;
        [SerializeField] private Material skyBox;

        public string Id => id;
        public Material SkyBox => skyBox;
    }

    [System.Serializable]
    public class TracksList
    {
        [SerializeField] private TextAsset trackData;
        [SerializeField] private Sprite spriteOfmap;
        [SerializeField] private bool isCircle = true;
        [SerializeField] private int laps;
        [ShowNativeProperty] public string Id => trackData.name;
        [ShowNativeProperty] public Reward RaceReward => Game.Config.statsConfig.GetReward(Id);

        public int Circles => isCircle ? laps : 1;
        public Sprite MapSprite => spriteOfmap;
        public TrackFullInfo trackParts => JsonUtility.FromJson<TrackFullInfo>(trackData.text);
        
    }

    [System.Serializable]
    public class Car
    {
        [SerializeField] private string id;
        [SerializeField] private GameObject carModel;
        [SerializeField] private PrometeoCarController carReferene;

        public string Id => id;
        public GameObject CardModel => carModel;
        public PrometeoCarController CarReference => carReferene;
    }
}

[System.Serializable]
public class Views
{
    [SerializeField] private CarUpgradeView carUpgradeView;
    public CarUpgradeView GetCarUpgradeView => carUpgradeView;

    [SerializeField] private SelectCarView selectCarView;
    public SelectCarView GetSelectCarView => selectCarView;

    [SerializeField] public TouchInputView touchInputView;
    public TouchInputView GetTouchInputView => touchInputView;

    [SerializeField] private ChooseMapSelectView chooseMapSelectView;
    public ChooseMapSelectView GetChooseMapSelectView => chooseMapSelectView;

    [SerializeField] private FinishCanvas finishCanvas;
    public FinishCanvas GetFinishCanvas => finishCanvas;

    [SerializeField] private RateWindow rateWindow;
    public RateWindow GetRateWindow => rateWindow;

}

[Serializable]
public class Icons
{
    [SerializeField] private List<CharacteristicIcon> iconsCharacteristics;
    [SerializeField] private List<UpgradedSprite> upgradedSprite;
    public Sprite GetSpriteCharacteristic(CharacteristicType type)
    {
        return iconsCharacteristics.Find(x => x.Type == type).Icon;
    }
    public Sprite GetUpgraded(bool isUpgraded)
    {
        return upgradedSprite.Find(x => x.IsUpgraded == isUpgraded).Sprite;
    }

    [Serializable]
    private class UpgradedSprite
    {
        [SerializeField] private bool isUpgraded;
        [SerializeField] private Sprite sprite;
    
        public bool IsUpgraded => isUpgraded;
        public Sprite Sprite => sprite;
    }

    [Serializable] 
    public class CharacteristicIcon
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private CharacteristicType type;

        public Sprite Icon => icon;
        public CharacteristicType Type => type;
    }
}

[System.Serializable]
public class RaceUI
{
    [SerializeField] private RaceType id;
    [SerializeField] private RaceUIComponent[] components;

    public RaceType Id => id;
    public RaceUIComponent[] Components => components.ToArray();
}