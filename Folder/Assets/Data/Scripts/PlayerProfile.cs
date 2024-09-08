using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConfig;

[Serializable]
public class PlayerProfile
{
    [SerializeField] private List<CarCharacteristics> characteristicsCar = new List<CarCharacteristics>();
    public Wallet wallet = new Wallet();

    [SerializeField, HideInInspector] private bool isInited = false;
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
    [SerializeField] private int softCurrency = 50000;
    public int SoftCurrency => softCurrency;
    public System.Action<int> OnCurrencyChanged;
    public bool SpendSoft(int value)
    {
        if (CheckSoftEnough(value))
        { 
            softCurrency -= value;
            OnCurrencyChanged?.Invoke(softCurrency);
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
        OnCurrencyChanged?.Invoke(softCurrency);
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

    public void UpgradeChraracteristic(CharacteristicType type, string carId)
    {
        var info = Game.Config.statsConfig.upgradeCosts.Find(x => x.Type == type && x.CarId == carId);
        if (!Game.Player.wallet.SpendSoft((int)info.Price))
            return;

        var characteristic = characteristics.Find(x => x.Type == type);
        if(characteristic is null)
        {
            characteristic = new CarCharacteristic(type, carId);
            characteristics.Add(characteristic);
        }
        characteristic.Upgrade();
    }
}

[System.Serializable]
public class CarCharacteristic
{
    [SerializeField] private int level = 1;
    [SerializeField] private CharacteristicType type;
    [SerializeField] private float calculatedPower;
    private string carId;
    private UpgradeCosts info;

    
    public int Cost 
    { 
        get 
        { 
            if(info is null)
                info = Game.Config.statsConfig.upgradeCosts.Find(x => x.Type == type && x.CarId == carId && x.Level == level + 1);
            return (int)info.Price; 
        } 
    }

    private const int MAX_LEVEL = 10;
    public bool IsMaxUpgrade => level >= MAX_LEVEL;
    public int Level => level;
    public CharacteristicType Type => type;
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
        calculatedPower = info.Bonus; //тут надо конфиг сделать будет

    }
}