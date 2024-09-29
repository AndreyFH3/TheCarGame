using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [Space(10)]
    [SerializeField] private AudioSource firstPlayer;
    [SerializeField] private AudioSource secondPlayer;
    [Space(10)]
    [SerializeField] private float timeToChangeTracks = 1.5f;
    
    private float currentTrackTime;
    private string currentSceneName = "";


    private static SoundPlayer player;
    public static SoundPlayer Player => player;

    private void Awake()
    {
        if(player is null)
        {
            player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Play(AudioClip music)
    {
        firstPlayer.Stop();
        secondPlayer.Stop();
        currentTrackTime = music.length - timeToChangeTracks;

        if (firstPlayer.isPlaying)
        {
            firstPlayer.clip = music;
            firstPlayer.Play();
        }
        else
        {
            secondPlayer.clip = music;
            secondPlayer.Play();
        }
    }

    public void PlayMenuMusic()
    {
        var music = Game.Config.SoundConfig.MenuSound;
        Play(music);
    }

    public void PlayRaceMusic()
    {
        var music = Game.Config.SoundConfig.RaceSound;
        Play(music);
    }

    private void Update()
    {
        var isClipNeedChange = currentTrackTime -= Time.deltaTime;
        if (isClipNeedChange < 0 || currentSceneName != SceneManager.GetActiveScene().name)
        {
            currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName.CompareTo(SceneNames.GARAGE_SCENE_SCENE) != 0)
                PlayRaceMusic();
            else
                PlayMenuMusic();
        }
    }

    public void SetMusic(bool value)
    {
        mixer.SetFloat("Music", value ? 1 : 0);
        Game.Player.settings.SetCarSoundSound(value);
    }

    public void SetCarSound(bool value)
    {
        mixer.SetFloat("CarSound", value ? 1 : 0);
        Game.Player.settings.SetMusic(value);
    }

}

public static class SceneNames
{
    public const string TRACK_SCENE = "Track";
    public const string START_SCENE_SCENE = "StartScene";
    public const string GARAGE_SCENE_SCENE = "GarageScene";
}