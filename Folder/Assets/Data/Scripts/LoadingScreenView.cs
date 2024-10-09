using GamePush;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenView : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (!Game.Instance.IsInit)
        {
            StartCoroutine(Game.Instance.Init());
        }
    }

    private void Update()
    {
        if (Game.Instance.IsInit)
        {
            slider.value += Time.deltaTime *.8f;
            if (slider.value >= 1)
            { 
                SceneManager.LoadScene(SceneNames.GARAGE_SCENE_SCENE);
                //GP_Game.GameReady();
                //GP_Ads.ShowFullscreen();
            }
        }
        else
        {
            slider.value += slider.value > .4f ? 0 : Time.deltaTime *.8f;
        }        
    }
}
