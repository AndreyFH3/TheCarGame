using UnityEngine;

public class SelectCarView : MonoBehaviour, ILookable
{
    public System.Action OnNextCar;
    public System.Action OnPreviousCar;
    private CarShower showerCars;
    private TrackLoader trackLoader;

    public Vector3 LookPosition => new Vector3();

    public void Init(CarShower shower)
    {
        showerCars = shower;
    }

    public void NextCarChoose()
    {
        OnNextCar?.Invoke();
    }

    public void PreviousCarChoose()
    {
        OnPreviousCar?.Invoke();
    }

    public void OpenCarUpgradeView()
    {
        var instance = Instantiate(Game.Config.views.GetCarUpgradeView);
        instance.Init(showerCars.SelectedCarId);
        instance.OnClose += () => { gameObject.SetActive(true); };
        gameObject.SetActive(false);
    }

    public void SelectTrack()
    {
        var item = new TrackLoader(showerCars.SelectedCarId);
        var instance = Instantiate(Game.Config.views.GetChooseMapSelectView);
        instance.OnClose += () => { gameObject.SetActive(true); };
        instance.Init(item);
        gameObject.SetActive(false);
    }

    public void LookAtTarget()
    {
        throw new System.NotImplementedException();
    }
}

public interface ILookable
{
    public Vector3 LookPosition { get; }

    public void LookAtTarget();
}