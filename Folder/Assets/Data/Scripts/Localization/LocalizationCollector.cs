using GamePush;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Localization", menuName = "ScriptableObjects/Localization", order = 2)]
public class LocalizationCollector : ScriptableObject
{
    public List<LanguageCollection> localizaions;
    public Dictionary<Language, Dictionary<string, string>> languages;

    [Button]
    public void CreateCollection()
    {
        languages = new();
        foreach (var locale in localizaions)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in locale.Collection)
            {
                dict.Add(item.Key, item.Locale);
            }
            languages.Add(locale.Language, dict);
        }
    }
}

public static class Localizationasdasd 
{
    public static Dictionary<Language, Dictionary<string, string>> languages;
    private static bool isInit = false;
    public static void Init()
    {
        if (isInit)
            return;
        var conf = Resources.Load("Localization") as LocalizationCollector;
        if(conf is null)
        {
            Debug.LogError("ERROR ON LOADING LOCALIZATION CONFIG");
            return;
        }
        if (conf.languages is not null)
            languages = new(conf.languages);
        else
            CreateCollection(conf.localizaions);
        isInit = true;
    }

    public static string Get(string key)
    {
        var current = GP_Language.Current();
        if(languages.ContainsKey(current) && languages[current].ContainsKey(key))
        {
            var res = languages[current][key];
            if (System.String.IsNullOrEmpty(res))
            {
                Debug.LogError($"Key not Found {key}");
                return key.ToUpper();
            }
            return res;
        }
        Debug.LogError($"Language not Found {current.ToString()}");
        return key.ToUpper();
    }

    public static void CreateCollection(List<LanguageCollection> localizaions)
    {
        languages = new();
        foreach (var locale in localizaions)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in locale.Collection)
            {
                dict.Add(item.Key, item.Locale);
            }
            languages.Add(locale.Language, dict);
        }
    }
}

[System.Serializable]
public class LanguageCollection
{
    [SerializeField] private Language language;
    [SerializeField] private List<LocaleCollection> collection;

    public Language Language => language;
    public List<LocaleCollection> Collection => collection;

    public string Get(string key)
    {
        var res = collection.Find(x => x.Key == key);
        if (System.String.IsNullOrEmpty(res.Locale))
            return key.ToUpper();
        return res.Locale;
    }
}

[System.Serializable]
public class LocaleCollection
{
    [SerializeField] private string key;
    [SerializeField] private string locale;

    public string Key => key;
    public string Locale => locale;
}

