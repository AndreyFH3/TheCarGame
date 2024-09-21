using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LocalizationConfig", menuName = "ScriptableObjects/LocalizationConfig", order = 2)]
public class LocalizationConfig : ScriptableObject
{
    [FormerlySerializedAs("CurrentLanguage")] public SystemLanguage currentLanguage;
    [FormerlySerializedAs("DefaultLanguage")] public SystemLanguage defaultLanguage;
    [FormerlySerializedAs("NotFoundPreffix")] public string notFoundPreffix;
    [FormerlySerializedAs("ShowKeysInsteadLocalizationInEditor")] public bool showKeysInsteadLocalizationInEditor;

    [FormerlySerializedAs("Urls")][SerializeField] List<string> urls;

    [FormerlySerializedAs("Keys")] public List<string> keys;
    [FormerlySerializedAs("Locales")] public List<Locale> locales;
    [System.Serializable]
    public class Locale
    {
        [FormerlySerializedAs("Language")] public SystemLanguage language;
        [FormerlySerializedAs("Title")] public string title;
        [FormerlySerializedAs("Icon")] public Sprite icon;
        [FormerlySerializedAs("Strings")] public List<string> strings;
        [FormerlySerializedAs("Enabled")] public bool enabled;
    }

    [System.NonSerialized]
    private Dictionary<string, int> _keyIndexes;
    [System.NonSerialized]
    private List<List<string>> _valuesByLanguage;

    public void Rebuild()
    {
        _keyIndexes = new Dictionary<string, int>();
        for (var i = 0; i < keys.Count; i++)
        {
            if (!_keyIndexes.ContainsKey(keys[i]))
            {
                _keyIndexes.Add(keys[i], i);
            }
            else
            {
                Debug.LogError("Duplicate key: " + keys[i]);
            }
        }

        _valuesByLanguage = new List<List<string>>();
        for (var i = 0; i <= (int)SystemLanguage.Unknown; i++)
        {
            var locale = locales.FirstOrDefault(l => (int)l.language == i);
            if (locale == null)
            {
                locale = locales.FirstOrDefault(l => l.language == defaultLanguage);
            }
            if (locale != null)
            {
                _valuesByLanguage.Add(locale.strings);
            }
            else
            {
                _valuesByLanguage.Add(new List<string>());
            }
        }
    }

    public string GetValue(string key, SystemLanguage language)
    {
#if UNITY_EDITOR
        if (showKeysInsteadLocalizationInEditor)
        {
            if (string.IsNullOrEmpty(key)) { return notFoundPreffix; }
            if (!_keyIndexes.ContainsKey(key))
            {
                return notFoundPreffix + key;
            }
            return "!!" + key;
        }
#endif

        if (string.IsNullOrEmpty(key)) { return notFoundPreffix; }
        if (_keyIndexes.TryGetValue(key, out int value))
        {
            return _valuesByLanguage[(int)language][value];
        }
#if UNITY_EDITOR
        if (_missingKeys.Add(key))
        {
            Debug.LogError(key);
        }
#endif
        return notFoundPreffix + key;
    }

    public Locale GetLocale(SystemLanguage language)
    {
        return locales.FirstOrDefault(item => item.language == language);
    }


    private HashSet<string> _missingKeys = new HashSet<string>();

    [Button]
    private void PrintMissingKeys()
    {
        var str = "";
        foreach (var k in _missingKeys)
        {
            str += k + "\n";
        }
        Debug.LogError(str);
    }

    [Button]
    public void DownloadLocalization()
    {
        var listKeys = new List<string>();
        var languages = new Dictionary<SystemLanguage, List<string>>();

        foreach (var url in urls)
        {
            var data = GoogleDocsDownloader.Download(url);
            var lines = GoogleDocsDownloader.CsvToArray(data);

            var langLine = lines[0];
            lines.RemoveAt(0);
            foreach (var line in lines)
            {
                listKeys.Add(line[0].ToUpper());
            }
            for (var i = 1; i < langLine.Length; i++)
            {
                var listToAdd = new List<string>();
                var langStr = langLine[i];
                SystemLanguage lang;
                if (System.Enum.TryParse(langStr, ignoreCase: true, out lang))
                {
                    listToAdd = languages.TryGetOrCreate(lang);
                }
                else
                {
                    Debug.LogError("Failed to find language " + langStr);
                }
                foreach (var line in lines)
                {
                    var str = line.GetSafe(i);
                    if (str == null) { str = ""; }
                    listToAdd.Add(str);
                }
            }
        }

        keys = listKeys;
        var oldLocales = locales;
        locales = new List<LocalizationConfig.Locale>();

        while (true)
        {
            var index = listKeys.FindIndex(s => string.IsNullOrEmpty(s));
            if (index >= 0)
            {
                listKeys.RemoveAt(index);
                foreach (var s in languages)
                {
                    s.Value.RemoveAt(index);
                }
            }
            else
            {
                break;
            }
        }

        foreach (var kv in languages)
        {
            var locale = new LocalizationConfig.Locale { language = kv.Key, strings = kv.Value };
            var oldLocale = oldLocales?.FirstOrDefault(l => l.language == kv.Key);
            if (oldLocale != null)
            {
                locale.title = oldLocale.title;
                locale.icon = oldLocale.icon;
                locale.enabled = oldLocale.enabled;
            }
            locales.Add(locale);
        }
        Rebuild();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

}

public static class StringExtension
{
    public static T GetSafe<T>(this T[] array, int x)
    {
        if (x < 0 || x >= array.Length)
        {
            return default(T);
        }
        return array[x];
    }
    public static V TryGetOrCreate<K, V>(this Dictionary<K, V> dict, K key)
    where V : new()
    {
        V val;
        if (!dict.TryGetValue(key, out val))
        {
            val = new V();
            dict.Add(key, val);
        }
        return val;
    }
}