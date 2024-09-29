using UnityEngine;

public class OpenSettings : MonoBehaviour
{
    public void OpenSettingsWindow()
    {
        var instance = Instantiate(Game.Config.views.GetSettingsView);
        instance.Init();
    }    
}
