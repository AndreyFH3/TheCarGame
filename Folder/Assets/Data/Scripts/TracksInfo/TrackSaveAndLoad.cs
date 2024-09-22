using GamePush;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackSaveAndLoad : MonoBehaviour
{

    [SerializeField] private Transform trackParent;
    [SerializeField] private Transform collidersParent;

    [SerializeField] private string trackName;
    public bool IsInited => isCarSet && isTimeSelected && isTrackSet;
    [SerializeField] private Transform raceUICanvas;

    private RaceController raceController;
    private bool isCarSet;
    private bool isTimeSelected;
    private bool isTrackSet;

    public RaceController Controller => raceController;

    private void Awake()
    {
        if(Game.Instance is null)
            return;
        raceController = RaceControllerCreator.GetController(Race.RaceType);

        if (Race.Settings is null && Controller is not null)
        {
            return;
        }
        SetDayTime(Race.Settings.isDay);
        var info = LoadTrack(Race.Settings.trackId);
        Controller.Init(
            SetCar(Race.Settings.carId, info.Last().transform.position, info[0].transform.position),
            info,
            Race.Settings);

        SetUI(raceController.RaceType);
    }

    [Button]
    public void CheckTrack()
    {
        List<SavableTransform> savableTransforms = new List<SavableTransform>();
        List<CollidersCheckers> collidersTransforms = new List<CollidersCheckers>();

        foreach (Transform child in trackParent)
        {
            if(child.TryGetComponent(out TrackPart part))
            {
                savableTransforms.Add(part.GetSaveData());
            }
        }
        foreach (Transform child in collidersParent)
        {
            if (child.TryGetComponent(out TrackWay part))
            {
                collidersTransforms.Add(part.GetSaveData());
            }
        }

        string jsonInfo = JsonUtility.ToJson(new TrackFullInfo(collidersTransforms, savableTransforms));
        string saveFilePath = $"{Application.dataPath}/{trackName}.json";
        if (System.IO.File.Exists(saveFilePath))
        {
            Debug.LogWarning("File Exists!");
        }
        System.IO.File.WriteAllText(saveFilePath, jsonInfo);
        Debug.Log($"File {trackName}.json was saved!\nSaved Path: {saveFilePath}");
    }

    [Button]
    public List<TrackWay> LoadTrack(string trackId = "", System.Action<int> action = null)
    {
        if (string.IsNullOrEmpty(trackId))
            trackId = trackName;
        var info = Game.Config.GetTrack(trackId).trackParts;
        foreach (var child in info.GetTransforms)
        {
            var trans = Game.Config.GetTrackPart(child.Id);
            Instantiate(trans, trackParent).SetSavedData(child);
        }
        var colliders = new List<TrackWay>();
        foreach (var child in info.GetColliders)
        {
            var obj = Instantiate(Game.Config.ColliderExample, collidersParent);
            obj.SetSavedData(child);
            obj.OnColliderTouched += action;
            colliders.Add(obj);
            //тут надо сделать какой-то метод, который будет передавать необходимую информацию
        }
        isTrackSet = true;
        return colliders;
    }


    public PrometeoCarController SetCar(string carId, Vector3 spawnPosiition, Vector3 lookAtPosition)
    {
        var carData = Instantiate(Game.Config.GetCar(carId).CarReference);
        carData.Init(Game.Config.statsConfig.GetCarSettings(Race.Settings.carId), GP_Device.IsDesktop());
        carData.transform.position = spawnPosiition;

        var lookPos = lookAtPosition;
        carData.transform.LookAt(new Vector3(lookPos.x, carData.transform.position.y, lookPos.z));

        Camera.main.transform.position = carData.transform.position + new Vector3(15, 10, 10);
        Camera.main.gameObject.AddComponent<CameraFollow>().Init(carData, 2, 5, .5f);

        isCarSet = true;
        return carData;
    }

    public void SetDayTime(bool isDay)
    {
        RenderSettings.skybox = Game.Config.GetSkyBox(isDay);
        isTimeSelected = true;
    }
    private void SetUI(RaceType type)
    {
        var components = Game.Config.GetRaceUICompnents(type);
        foreach (var component in components)
        {
            var instance = Instantiate(component, raceUICanvas);
            instance.Init(raceController);
        }
    }
}