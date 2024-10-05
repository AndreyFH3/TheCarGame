using GamePush;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveAndLoad
{
    private const string ACCOUNT = "Account";

    //private static List<PlayerFetchFieldsData> fields;

    //public static List<PlayerFetchFieldsData> Fields => fields;

    public static void SaveProife()
    {
        var profile = Game.Player;
        var stringToSave = JsonUtility.ToJson(profile);
#if !UNITY_EDITOR
        GP_Player.Set(ACCOUNT, stringToSave);
        GP_Player.Sync();

#endif
#if UNITY_EDITOR
        string saveFilePath = $"{Application.dataPath}/PlayerProfile.json";
        if (System.IO.File.Exists(saveFilePath))
        {
            Debug.LogWarning("File Exists!");
        }
        System.IO.File.WriteAllText(saveFilePath, stringToSave);
        Debug.Log($"File stringToSave.json was saved! Saved Path: {saveFilePath}");
#endif
    }

    public static void ResetSaves()
    {
        string saveFilePath = $"{Application.dataPath}/PlayerProfile.json";

        if (System.IO.File.Exists(saveFilePath))
        {
            Debug.LogWarning("File Exists!");
            GP_Player.ResetPlayer();    
        }
    }

    public static PlayerProfile LoadPlayer()
    {
#if UNITY_EDITOR
        var profileString = "";
        string loadFilePath = $"{Application.dataPath}/PlayerProfile.json";

        if (System.IO.File.Exists(loadFilePath))
        {
            profileString = System.IO.File.ReadAllText(loadFilePath);
            var profile = JsonUtility.FromJson<PlayerProfile>(profileString);
            return profile;
        }
        return new();
#endif
#if !UNITY_EDITOR
        var profileString = GP_Player.GetString(ACCOUNT);
        var profile = JsonUtility.FromJson<PlayerProfile>(profileString);
        return profile;
#endif


    }
}
