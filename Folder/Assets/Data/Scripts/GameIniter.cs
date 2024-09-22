using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIniter : MonoBehaviour
{
    void Start()
    {
        if (Game.Instance is null )
        {
            //SceneManager.LoadScene(0);
        }
    
    }
}
