using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public static class Localization
{
    public static SystemLanguage Language => Config.currentLanguage;
    public static List<LocalizationConfig.Locale> Locales => Config.locales.Where(l => l.enabled).ToList();
    public static event Action OnLanguageChange;

    private static LocalizationConfig _cachedConfig;

    private static LocalizationConfig Config
    {
        get
        {
            if (!_cachedConfig)
            {
                _cachedConfig = Resources.Load<LocalizationConfig>("LocalizationConfig");
                _cachedConfig.Rebuild();
            }
            return _cachedConfig;

        }
    }

    //------------------------------------------------------------

    public static string Get(string key)
    {
        return Config.GetValue(key?.ToUpper(), Language);
    }

    public static string Get(string key, params object[] args)
    {
        return string.Format(Config.GetValue(key?.ToUpper(), Language), args);
    }

    public static LocalizationConfig.Locale GetLocale()
    {
        return Config.GetLocale(Language);
    }

    public static void Load()
    {
        var language = (SystemLanguage)PlayerPrefs.GetInt("Language", (int)Config.defaultLanguage);/*Config.DefaultLanguage;*/
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Locales.Any(l => l.language == Application.systemLanguage))
            {
                language = Application.systemLanguage;
            }
            SetLanguage(language);
        }
        Config.currentLanguage = language;

        OnLanguageChange?.Invoke();
    }

    public static void SetLanguage(SystemLanguage language)
    {
        PlayerPrefs.SetInt("Language", (int)language);
        Config.currentLanguage = language;
        OnLanguageChange?.Invoke();
        PlayerPrefs.Save();
    }
}

public interface ILocalizationContent
{
    void RefreshLocalizationContent();
}