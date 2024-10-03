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
            Game.Instance.Init();
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
            }

        }
        else
        {
            slider.value = 0;
        }        
    }
}
