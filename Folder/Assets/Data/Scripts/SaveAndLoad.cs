using GamePush;
using UnityEngine;

public static class SaveAndLoad
{
    private const string ACCOUNT = "Account";
    public static void SaveProife()
    {
        var profile = Game.Player;
        var stringToSave = JsonUtility.ToJson(profile);
        GP_Player.Set(ACCOUNT, stringToSave);
        GP_Player.Sync(forceOverride: true);
    }

    public static void ResetSaves()
    {
        GP_Player.ResetPlayer();    
    }

    public static PlayerProfile GetPlayer()
    {
        var accountInfo = GP_Player.GetString(ACCOUNT);
        if (System.String.IsNullOrEmpty(accountInfo))
        {
            PlayerProfile profile = JsonUtility.FromJson<PlayerProfile>(accountInfo);
            return profile;
        }
        return null;
    }
}
