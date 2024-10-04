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
        GP_Player.Set(ACCOUNT, stringToSave);
        GP_Player.Sync();
    }

    public static void ResetSaves()
    {
        GP_Player.ResetPlayer();    
    }

    public static PlayerProfile LoadPlayer()
    {
        var profileString = GP_Player.GetString(ACCOUNT);
        var profile = JsonUtility.FromJson<PlayerProfile>(profileString);
        return profile;

    }
}
