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
    public int lastPointIndex { get; protected set; } = -1;
    
    public System.Action OnFinish;
    public System.Action<bool> OnPlayerWrongWay;

    public bool isWrongWay { get; protected set; } = false;
    public float TimeWrongDistance { get; protected set; } = 5f;
    public float timeWrongDistance = 5f;

    public PrometeoCarController CarController { get; protected set; }
    public DefaultRaceSettings RaceSettings => settings;
    protected DefaultRaceSettings settings;

    public virtual void Init(PrometeoCarController carController, List<TrackWay> countedCollider, DefaultRaceSettings settings) 
    { 
        CarController = carController; 
        TrackWays = countedCollider; 
        this.settings = settings;

        TrackWays.ForEach(x => x.OnFixedUpdate += CalculatePosition);

        var characteristics = Game.Player.GetCarCharacteristics(settings.carId);
        CarController.maxSpeed += (int)characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.speed).CalculatedPower;
        CarController.accelerationMultiplier+= (int)characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.boost).CalculatedPower;
        CarController.brakeForce += (int)characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.controllability).CalculatedPower;
        CarController.maxSteeringAngle += (int)characteristics.Characteristics.ToList().Find(x => x.Type == CharacteristicType.controllability).CalculatedPower;
    }
    
    public void StartRace()
    {
        IsStarted = true;
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
            if (distance + 50 >= distanceToNextCheckPoint)
            {
                timeWrongDistance += Time.fixedDeltaTime;
            }
            if(timeWrongDistance >= TimeWrongDistance)
            {
                isWrongWay = true;
                Debug.Log(isWrongWay);
            }
        } 
    
        OnPlayerWrongWay?.Invoke(isWrongWay);
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
    private int passedLaps = 1;
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
        StartTime = Time.time;
        for (int i = 0; i < TrackWays.Count; i++)
        {
            TrackWays[i].OnColliderTouched += OnControlPointPassed;
            TrackWays[i].gameObject.SetActive(false);
        }
        times = Game.Config.statsConfig.GetTrackTimes(Settings.trackId);
        TrackWays[0].gameObject.SetActive(true);
        laps = Settings.cirlces;
    }

    public override void OnControlPointPassed(int ind)
    {
        if (ind <= 0) 
            ind = 1;
        
        else if (ind >= TrackWays.Count) 
        {
            TrackWays[TrackWays.Count - 1].gameObject.SetActive(false);
            TrackWays[0].gameObject.SetActive(true);

            if (passedLaps >= laps)
            {
                Finish();
                return;
            }
            else
            {
                passedLaps++;
                OnLapPassed?.Invoke(passedLaps);
                ind = 1;
            }
        }

        TrackWays[ind - 1].gameObject.SetActive(false);
        TrackWays[ind].gameObject.SetActive(true);
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
        StartTime = Time.time;
        for (int i = 0; i < TrackWays.Count; i++)
        {
            TrackWays[i].OnColliderTouched += OnControlPointPassed;
            TrackWays[i].gameObject.SetActive(false);
        }
        trackInfo = Game.Config.statsConfig.GetTrackPoints(Settings.trackId);
        TrackWays[0].gameObject.SetActive(true);
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
        if (ind <= 0)
            ind = 1;

        else if (ind >= TrackWays.Count)
        {
            TrackWays[TrackWays.Count - 1].gameObject.SetActive(false);
            TrackWays[0].gameObject.SetActive(true);

            if (passedLaps >= laps)
            {
                Finish();
                return;
            }
            else
            {
                passedLaps++;
                OnLapPassed?.Invoke(passedLaps);
                ind = 1;
            }
        }

        TrackWays[ind - 1].gameObject.SetActive(false);
        TrackWays[ind].gameObject.SetActive(true);
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