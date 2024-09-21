using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class LocalizationTMProTextUI : MonoBehaviour
{
    [FormerlySerializedAs("String")][SerializeField] LocalizedString @string;
    TMPro.TextMeshProUGUI _text;
    [ShowNativeProperty] public string Value => @string.Value;

    private void OnEnable()
    {
        Refresh();
        Localization.OnLanguageChange += Refresh;
    }

    public void Refresh()
    {
        if (!_text) { _text = GetComponent<TMPro.TextMeshProUGUI>(); }
        _text.text = @string;
    }

    private void OnDisable()
    {
        Localization.OnLanguageChange -= Refresh;
    }
}

[System.Serializable]
public struct LocalizedString
{
    [ShowNativeProperty] public string Value => Localization.Get(key); 

    public string key;
    public bool HasText { get { return !string.IsNullOrEmpty(key); } }

    public static implicit operator string(LocalizedString s)
    {
        return s.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}