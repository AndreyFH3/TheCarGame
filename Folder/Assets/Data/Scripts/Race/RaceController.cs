using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public abstract class RaceController
{
    public abstract RaceType RaceType { get; }
    public abstract bool Pause { get; protected set; }
    public virtual bool IsStarted { get; protected set; }
    public float StartTime { get; protected set; }
    public abstract float RaceTime { get; }
    public List<TrackWay> TrackWays { get; protected set; }
    public float distanceToNextCheckPoint { get; protected set; } = float.MaxValue;
    public float distanceWrongDirection { get; protected set; } 
    public float distanceBetweenWrongAndLastCorrect { get; protected set; } 
    public int lastPointIndex { get; protected set; } = -1;
    
    public System.Action OnFinish;
    public System.Action<bool> OnPlayerWrongWay;

    public bool isWrongWay { get; protected set; } = true;
    public float TimeWrongDistance { get; protected set; } = 10f;
    public float timeWrongDistance = 0f;

    public float TimeCorrectInWrongDistance { get; protected set; } = 3f;
    public float timeCorrectInWrongDistance = 0f;

    protected int lastPassedPoint = -1;
    protected int lastWrongPassedPoint = -1;

    public PrometeoCarController CarController { get; protected set; }
    public DefaultRaceSettings RaceSettings => settings;
    protected DefaultRaceSettings settings;

    public virtual void Init(PrometeoCarController carController, List<TrackWay> countedCollider, DefaultRaceSettings settings) 
    { 
        CarController = carController; 
        TrackWays = countedCollider; 
        this.settings = settings;
        lastPassedPoint = TrackWays.Count - 1;
        TrackWays.ForEach(x => x.OnTouchColliderCalculate += CalculatePosition);

        var characteristics = Game.Player.GetCarCharacteristics(settings.carId);
        CarController.maxSpeed += (int)characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.speed).CalculatedPower;
        CarController.accelerationMultiplier += (int)characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.boost).CalculatedPower;
        CarController.handbrakeDriftMultiplier = characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.controllability).CalculatedPower;
    }
    
    public void StartRace()
    {
        IsStarted = true;
        StartTime = Time.time;
    }


    public void FinishRace()
    {

    }

    public abstract void OnControlPointPassed(int ind);
    public virtual void SetPaused() 
    { 
        Pause = true;
        Time.timeScale = 0.0f;
    }

    public virtual void SetUnpaused() 
    { 
        Pause = false;
        Time.timeScale = 1.0f;
    }

    protected virtual void Finish() 
    {
        CarController.enabled = false;
        OnFinish?.Invoke();
        SaveAndLoad.SaveProife();
    }

    private void CalculatePosition(int counted)
    {
        var isCorrectNow = true;
        if(IsMovingForward(counted))
        {
            isWrongWay = false;
        }
        else
        {
            isWrongWay = true;

            if (lastWrongPassedPoint - 1 == counted || counted == TrackWays.Count)
            {
                isCorrectNow = false;
            }
            lastWrongPassedPoint = counted;
        }   
        /*
        if (lastPointIndex != counted)
        {
            lastPointIndex = counted;
            distanceToNextCheckPoint = float.MaxValue;
        }
        if (counted >= TrackWays.Count)
            counted = 0;

        var distance = Vector3.Distance(TrackWays[counted].transform.position, CarController.transform.position);
        if (distance <= distanceToNextCheckPoint)
        {
            timeWrongDistance = 0;
            isWrongWay = false;
            distanceToNextCheckPoint = distance;
            Debug.Log(isWrongWay);
        }

        else
        {
            if (distance + 200 >= distanceToNextCheckPoint)
            {
                timeWrongDistance += Time.fixedDeltaTime;
            }
            if(timeWrongDistance >= TimeWrongDistance)
            {
                distanceWrongDirection = distance;
                if(distanceBetweenWrongAndLastCorrect > distanceWrongDirection - distanceToNextCheckPoint)
                {
                    timeCorrectInWrongDistance += Time.deltaTime;
                    if(timeCorrectInWrongDistance >= TimeCorrectInWrongDistance)
                    {
                        distanceToNextCheckPoint = distance;
                        timeCorrectInWrongDistance = 0;
                    }
                }

                distanceBetweenWrongAndLastCorrect = distanceWrongDirection - distanceToNextCheckPoint;
                isWrongWay = true;
                Debug.Log(isWrongWay);
            }
        } 
    */
        OnPlayerWrongWay?.Invoke(!isCorrectNow);
        bool IsMovingForward(int currentCheckpointIndex)
        {
            // Если текущая точка больше последней, то двигаемся вперед
            if (currentCheckpointIndex == lastPassedPoint)
            {
                return true;
            }

            // Если текущая точка - 0, а последняя точка - это последний чекпоинт, то игрок завершает круг
            if (currentCheckpointIndex == TrackWays.Count && lastPassedPoint == 0)
            {
                return true;
            }

            // В остальных случаях игрок движется назад
            return false;
        }
    }


    public virtual void ExitRace() { UnityEngine.SceneManagement.SceneManager.LoadScene(1); Time.timeScale = 1; Pause = true; }

    public abstract int GetEarn();
}
[System.Serializable]
public class CircleRaceController : RaceController
{
    public override bool Pause { get; protected set; }
    public override RaceType RaceType { get; } = RaceType.defaultRace;
    public override float RaceTime  => Time.time - StartTime;
    private int laps = 1;
    private int passedLaps = 0;
    public int Laps => laps;
    private TrackInfo times;
    protected CircleRaceSettings Settings => (CircleRaceSettings)settings;

    public System.Action<int> OnLapPassed;

    public override void ExitRace()
    {
        base.ExitRace();
    }

    public override void Init(PrometeoCarController carController, List<TrackWay> countedCollider , DefaultRaceSettings settings)
    {
        base.Init(carController, countedCollider, settings);
        for (int i = 0; i < TrackWays.Count; i++)
        {
            TrackWays[i].OnColliderTouched += OnControlPointPassed;
        }
        times = Game.Config.statsConfig.GetTrackTimes(Settings.trackId);
        laps = Settings.cirlces;
    }

    public override void OnControlPointPassed(int ind)
    {
        if (lastPassedPoint + 1 == TrackWays.Count)
        {
            lastPassedPoint = 0;
            if (passedLaps >= laps)
            {
                Finish();
                return;
            }
            else
            {
                passedLaps++;
                OnLapPassed?.Invoke(passedLaps);
            }
        }
        else if (lastPassedPoint + 1 == ind)
        {
            lastPassedPoint = ind;
        }
    }

    protected override void Finish()
    {
        base.Finish();
        Game.Player.wallet.EarnSoft(GetEarn());
        var instance = GameObject.Instantiate(Game.Config.views.GetFinishCanvas);
        instance.Init(this);
    }

    public override int GetEarn()
    {
        if (times.ThreeStar >= RaceTime)
        {
            return Settings.reward.AmountThree;
        }
        else if (times.TwoStar >= RaceTime)
        {
            return Settings.reward.AmountTwo;
        }
        else 
            return Settings.reward.AmountOne;
    }
}

[System.Serializable]
public class DriftRaceController : RaceController
{
    private float driftPoints;
    public override RaceType RaceType => RaceType.driftRace;
    private int passedLaps = 1;
    private TrackInfo trackInfo;
    private int laps = 1;

    public override float RaceTime => Time.time - StartTime;
    public override bool Pause { get; protected set; }
    public int Laps => laps;
    public float DriftPoints => driftPoints;
    private DriftRaceSettings Settings => (DriftRaceSettings)settings;

    public System.Action<float> OnDrift;
    public System.Action<int> OnLapPassed;

    public override void Init(PrometeoCarController carController, List<TrackWay> countedCollider, DefaultRaceSettings settings)
    {
        base.Init(carController, countedCollider, settings);
        carController.OnDrift += CalculatePoints;
        for (int i = 0; i < TrackWays.Count; i++)
        {
            TrackWays[i].OnColliderTouched += OnControlPointPassed;
        }
        trackInfo = Game.Config.statsConfig.GetTrackPoints(Settings.trackId);
        laps = Settings.cirlces;
    }

    public void CalculatePoints()
    {
        if (Pause || isWrongWay)
            return;
        driftPoints += CarController.carSpeed;
        OnDrift?.Invoke(DriftPoints);
    }

    public override int GetEarn()
    {
        if(driftPoints >= trackInfo.ThreeStar)
        {
            return Settings.reward.AmountThree;
        }
        else if(driftPoints >= trackInfo.TwoStar)
        {
            return Settings.reward.AmountTwo;
        }
        else if (driftPoints >= trackInfo.OneStar)
        {
            return Settings.reward.AmountOne;
        }
        else
        {
            return 0;
        }
    }
    protected override void Finish()
    {
        base.Finish();
        Game.Player.wallet.EarnSoft(GetEarn());
        var instance = GameObject.Instantiate(Game.Config.views.GetFinishCanvas);
        instance.Init(this);
    }

    public override void OnControlPointPassed(int ind)
    {
        if (ind < 0)
            ind = 0;

        else if (ind >= TrackWays.Count)
        {
            //вот тут надо как то переделать систему подсчета

            if (passedLaps >= laps)
            {
                Finish();
                return;
            }
            else
            {
                passedLaps++;
                OnLapPassed?.Invoke(passedLaps);
                ind = 0;
            }
        }
        lastPassedPoint = ind;
    }
}


public enum RaceType { defaultRace = 1, driftRace = 2 }
public static class RaceControllerCreator
{
    public static RaceController GetController(RaceType race)
    {
        switch (race)
        {
            case RaceType.defaultRace:
                return new CircleRaceController();
            case RaceType.driftRace:
                return new DriftRaceController();
            default:
                Debug.LogError($"{race} is not implemented!");
                return null;
        }
    }
}