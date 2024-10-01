using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private Toggle carSound;
    [SerializeField] private Toggle music;

    [SerializeField] private TMP_Dropdown graphicsValues;

    private Settings Settings => Game.Player.settings;

    public void Init()
    {
        int graphicsValue = Settings.GraphicsValue;
        if (graphicsValue < 0)
        {
            graphicsValue = QualitySettings.GetQualityLevel();
        }
        graphicsValues.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var graphics in QualitySettings.names)
        {
            options.Add(new TMP_Dropdown.OptionData(Localization.Get(graphics)));
        }
        graphicsValues.AddOptions(options);
        graphicsValues.value = graphicsValue;
    }
    
    public void SetMusic(bool value)
    {
        SoundPlayer.Player.SetMusic(value);
    }

    public void SetCarSound(bool value)
    {
        SoundPlayer.Player.SetCarSound(value);
    }

    public void SetGraphics(int value)
    {
        Game.Player.settings.SetGraphics(value);
        QualitySettings.SetQualityLevel(value);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}

[System.Serializable]
public class Settings
{
    [SerializeField] private bool carSoundValue = true;
    [SerializeField] private bool musicValue = true;

    [SerializeField] private int graphicsValue = -1;
    [SerializeField] private int lastChoosedCar = -1;

    public bool CarSound => carSoundValue;
    public bool MusicValue => musicValue;
    public int GraphicsValue => graphicsValue;
    public int LastChoosedCar => lastChoosedCar;

    public void Init()
    {
        SoundPlayer.Player.SetMusic(MusicValue);
        SoundPlayer.Player.SetCarSound(CarSound);
        QualitySettings.SetQualityLevel(graphicsValue);
    }
    
    public void SetLastChoosedCar(int value)
    {
        lastChoosedCar = value;
    }

    public void SetGraphics(int value)
    {
        graphicsValue = value;
    }

    public void SetCarSoundSound(bool value)
    {
        carSoundValue = value;
    }

    public void SetMusic(bool value) 
    {
        musicValue = value;
    }
}