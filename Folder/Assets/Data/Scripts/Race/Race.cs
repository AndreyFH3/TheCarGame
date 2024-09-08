using System.Linq;
using UnityEngine;

[System.Serializable]
public static class Race
{
    [SerializeField] private static DefaultRaceSettings settings;
    public static DefaultRaceSettings Settings => settings;

    public static void SetSettings(DefaultRaceSettings settings) => Race.settings = settings;

    public static RaceType RaceType => settings.raceType;

}

[System.Serializable]
public class DefaultRaceSettings
{
    public string carId;
    public string trackId;
    public bool isDay;
    public Reward reward;
    public RaceType raceType = RaceType.defaultRace;
}

public class CircleRaceSettings : DefaultRaceSettings
{
    public int cirlces;
} 

public class DriftRaceSettings : DefaultRaceSettings
{ 
    public int cirlces;
}