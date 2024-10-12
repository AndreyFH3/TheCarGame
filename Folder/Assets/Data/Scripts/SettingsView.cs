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
    [SerializeField] private TMP_Dropdown languagesValues;

    [SerializeField] private List<SystemLanguage> languages;

    private Settings Settings => Game.Player.settings;

    public void Init()
    {
        carSound.isOn = Game.Player.settings.CarSound;
        music.isOn = Game.Player.settings.MusicValue;

        SetGraphicsValues();

        languagesValues.ClearOptions();
        var options2 = new List<TMP_Dropdown.OptionData>();
        int index = -1;
        foreach (var language in languages)
        {
            options2.Add(new TMP_Dropdown.OptionData(Localization.Get(language.ToString())));
        }
        languagesValues.AddOptions(options2);
        index = languages.FindIndex(x => x == Localization.Language);
        languagesValues.value = index;
    }

    public void OpenHowToPlay()
    {
        Instantiate(Game.Config.views.GetHowToPlayView);
    }

    private void SetGraphicsValues()
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

    private void OnEnable()
    {
        Localization.OnLanguageChange += SetGraphicsValues;   
    }

    private void OnDisable()
    {
        Localization.OnLanguageChange -= SetGraphicsValues;   
    }

    public void SetMusic(bool value)
    {
        SoundPlayer.Player.SetMusic(value);
        
    }

    public void SetLanguage(int index)
    {
        Localization.SetLanguage(languages[index]);
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
        SaveAndLoad.SaveProife();
    }

    public void SetCarSoundSound(bool value)
    {
        carSoundValue = value;
        SaveAndLoad.SaveProife();
    }

    public void SetMusic(bool value) 
    {
        musicValue = value;
        SaveAndLoad.SaveProife();
    }
}