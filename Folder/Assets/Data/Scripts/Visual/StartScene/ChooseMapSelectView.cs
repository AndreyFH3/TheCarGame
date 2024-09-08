using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMapSelectView : MonoBehaviour
{
    [SerializeField] private MapRaceView mapSpriteReference;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private HorizontalLayoutGroup hlg;
    [SerializeField] private ScrollRect scrollRect;

    //=====================================================================================================================================================================
    [Range(0, 100)]
    [SerializeField] private float snapForce = 10;
    [SerializeField] private float space = 10;
    private float snapSpeed;
    private float width;
    public int CurrentItem { 
        get => currentItem;
        protected set
        {
            if (currentItem != value)
            {
                Debug.Log(value);
                currentItem = value;
            }
        }
    }
    private int currentItem;
    public bool IsSnapped { get; private set; }

    private TrackLoader trackLoader;

    public System.Action OnClose;

    public void Init(TrackLoader loder)
    {
        IsSnapped = false;
        var widthComponentSetter = mapSpriteReference.transform as RectTransform;
        if(widthComponentSetter != null )
        {
            width = widthComponentSetter.rect.width;
        }

        trackLoader = loder;
        var tracks = Game.Config.GetTrackIds();
        for (int i = 0; i < tracks.Count; i++)
        {
            var instance = Instantiate(mapSpriteReference, contentPanel);
            instance.SetMapImage(Game.Config.GetTrack(tracks[i]).MapSprite);
        }
        mapSpriteReference.gameObject.SetActive(false);        
    }

    private void Update()
    {
        CurrentItem = Mathf.RoundToInt((-contentPanel.localPosition.x + 500) / (width + hlg.spacing));

        if (scrollRect.velocity.magnitude < 200 && !IsSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;

            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, 750 - (currentItem * (width + hlg.spacing)), snapSpeed),
                contentPanel.localPosition.y,
                contentPanel.localPosition.z
                );

            if (contentPanel.localPosition.x ==  -750 - (currentItem * (width + hlg.spacing)))
            {
                IsSnapped = true;
            }
        }

        else if (scrollRect.velocity.magnitude > 200)
        {
            IsSnapped = false;
            snapSpeed = 0;
        }
    }

    public void StartCircleRace()
    {
        trackLoader.StartCirlceRace(Game.Config.GetTrackIds()[CurrentItem - 1]);
    }
    public void StartDriftRace()
    {
        trackLoader.StartDriftRace(Game.Config.GetTrackIds()[CurrentItem - 1]);
    }

    private void CreateRaceSettings()
    {

    }

    public void Close()
    {
        OnClose?.Invoke();
        Destroy(gameObject);
    }
}



[System.Serializable]
public class TrackLoader
{
    private string CarId { get; set; }

    public TrackLoader(string carId)
    {
        SetCarId(Game.Config.GetCar(carId));
    }

    public void SetCarId(GameConfig.Car carInfo)
    {
        if(carInfo is not null)
            CarId = carInfo.Id;
    }

    public void StartCirlceRace(string raceId)
    {
        var settings = new CircleRaceSettings()
        {
            isDay = false,
            carId = CarId,
            trackId = raceId,
            raceType = RaceType.defaultRace
        };
        var c = Game.Config.GetTrack(raceId);
        settings.reward = c.RaceReward;
        settings.cirlces = c.Circles;
        Race.SetSettings(settings);
        SceneManager.LoadScene(2);
    }

    public void StartDriftRace(string raceId)
    {
        var settings = new DriftRaceSettings()
        {
            isDay = false,
            carId = CarId,
            trackId = raceId,
            raceType = RaceType.driftRace
        };
        var c = Game.Config.GetTrack(raceId);
        settings.reward = c.RaceReward;
        settings.cirlces = c.Circles;
        Race.SetSettings(settings);
        SceneManager.LoadScene(2);
    }
}