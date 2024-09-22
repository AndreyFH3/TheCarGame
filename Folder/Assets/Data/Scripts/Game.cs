using GamePush;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private static GameConfig config;

    private bool isInit = false;
    private static Game _instance;
    private const string PLAYER_DATA = "PLAYER_DATA";
    public static Game Instance => _instance;
    public bool IsInit => isInit;
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

    private static PlayerProfile playerContainer; 
    public static PlayerProfile Player {
        get 
        {
            if(playerContainer is not null && playerContainer.IsInited)
                return playerContainer;
            else
            {
                playerContainer = SaveAndLoad.GetPlayer();
                if(playerContainer is null)
                    playerContainer = new();
                else if(!playerContainer.IsInited)
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
        if(Instance is not null && !Instance.IsInit)
        {
            Player.Init();
            isInit = true;
            SceneManager.LoadScene(1);
        }
    }

    private void OnDataLoad()
    {
        Player.Init();
    }
}
