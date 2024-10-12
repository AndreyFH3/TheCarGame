using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerProfile", menuName = "ScriptableObjects/PlayerProfile", order = 2)]
[Serializable]
public class PlayerProfile
{
    [SerializeField] private List<CarCharacteristics> characteristicsCar = new List<CarCharacteristics>();
    [SerializeField] public CarShop carShop = new CarShop()
    {
       carIds = new List<string>()
       {
           "DefaultCar"
       }
    };
    [SerializeField] public MapShop mapShop = new MapShop()
    {
        mapsIds = new List<string>()
       {
           "FirstRace"
       }
    };
    [SerializeField] public Settings settings = new Settings();

    public Wallet wallet = new Wallet();
    public RateGame rate = new RateGame();

    [SerializeField] private bool isInited = false;
    [SerializeField] public bool isShowedHowTo = false;
    public bool IsInited => isInited;
    public CarCharacteristics GetCarCharacteristics(string carId) => characteristicsCar.Find(x => x.Id == carId);
    public void Init()
    {
        if (isInited) 
            return;
        else
        {
            foreach(var car in Game.Config.GetCars)
            {
                characteristicsCar.Add(new CarCharacteristics(car.Id));
            }
            isInited = true;
        }
    }
}

[System.Serializable]
public class Wallet
{
    [SerializeField] private int softCurrency = 0;
    public int SoftCurrency => softCurrency;
    public System.Action OnCurrencyChanged;
    public bool SpendSoft(int value)
    {
        if (CheckSoftEnough(value))
        { 
            softCurrency -= value;
            OnCurrencyChanged?.Invoke();
            SaveAndLoad.SaveProife();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckSoftEnough(int value)
    {
        return softCurrency - value >= 0;
    }

    public void EarnSoft(int value)
    {
        softCurrency += value;
        SaveAndLoad.SaveProife();
        OnCurrencyChanged?.Invoke();
    }
}

public enum CharacteristicType { speed = 0, boost = 1, controllability = 2 }

[System.Serializable]
public class CarCharacteristics
{
    [SerializeField] private string carId;
    [SerializeField] private List<CarCharacteristic> characteristics = new List<CarCharacteristic>();
    public string Id => carId;
    public CarCharacteristic[] Characteristics => characteristics.ToArray();
    
    public CarCharacteristics(string id)
    {
        this.carId = id;
        foreach(CharacteristicType characteristicType in Enum.GetValues(typeof(CharacteristicType)))
        {
            var characteristic = characteristics.Find(x => x.Type == characteristicType);
            if (characteristic is null)
            {
                characteristic = new CarCharacteristic(characteristicType, id);
                characteristics.Add(characteristic);
            }
        }
    }
}

[System.Serializable]
public class CarCharacteristic
{
    [SerializeField] private int level = 1;
    [SerializeField] private CharacteristicType type;
    [SerializeField] private float calculatedPower;
    [SerializeField] private string carId;

    private UpgradeCosts info;

    public bool CanUpgrade => Game.Player.wallet.CheckSoftEnough(Cost);
    
    public int Cost 
    { 
        get 
        { 
            if(info is null)
                info = Game.Config.statsConfig.upgradeCosts.Find(x => x.Type == type && x.CarId == carId && x.Level == level + 1);
            if(info is null)
                return int.MaxValue;
            return (int)info.Price; 
        } 
    }

    private const int MAX_LEVEL = 10;
    public bool IsMaxUpgrade => level >= MAX_LEVEL;
    public int Level => level;
    public CharacteristicType Type => type;
    public string CarId => carId;
    public float CalculatedPower => calculatedPower;

    public CarCharacteristic(CharacteristicType type, string carId)
    {
        this.type = type;
        this.carId = carId;
    }
    
    public void Upgrade()
    {
        if (!Game.Player.wallet.SpendSoft((int)info.Price))
            return;
        level++;
        if(MAX_LEVEL < level)
            level = MAX_LEVEL;
        info = Game.Config.statsConfig.upgradeCosts.Find(x => x.Type == type && x.CarId == carId && x.Level == level+1);
        if(info is not null)
            calculatedPower = info.Bonus;
        SaveAndLoad.SaveProife();
    }
}