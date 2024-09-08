using GamePush;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private static GameConfig config;
    private static Game _instance;
    private const string PLAYER_DATA = "PLAYER_DATA";
    public static GameConfig Config 
    {
        get
        {
            if (config == null)
            {
                config = Resources.Load("GameConfig") as GameConfig;
            }
            return config;
        }
    }

    public static PlayerProfile playerContainer; 
    public static PlayerProfile Player {
        get 
        {
            if(playerContainer is not null && playerContainer.IsInited)
                return playerContainer;
            else
            {
                string data = "";
                //data = GP_Player.GetString(PLAYER_DATA);
                if (!string.IsNullOrEmpty(data))
                {
                    playerContainer = JsonUtility.FromJson<PlayerProfile>(data);
                }
                else
                    playerContainer = new PlayerProfile();
                if(!playerContainer.IsInited)
                    playerContainer.Init();
                return playerContainer;
            }
        }
    }

    private void Start()
    {
        if (_instance is null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Player.Init();
    }

    private void OnDataLoad()
    {
        Player.Init();
    }
}
