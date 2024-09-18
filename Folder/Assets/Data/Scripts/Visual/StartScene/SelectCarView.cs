using UnityEngine;

public class SelectCarView : MonoBehaviour
{
    [SerializeField] private RectTransform boughtUI;
    [SerializeField] private RectTransform toBuyUI;
    [SerializeField] private RewardView priceShower;


    public System.Action OnNextCar;
    public System.Action OnPreviousCar;
    private CarShower showerCars;
    private CarShop shop => Game.Player.carShop;

    public void Init(CarShower shower)
    {
        showerCars = shower;
    }

    public void NextCarChoose()
    {
        OnNextCar?.Invoke();
        SetUICondition();
    }

    public void PreviousCarChoose()
    {
        OnPreviousCar?.Invoke();
        SetUICondition();
    }

    private void SetUICondition()
    {
        if (shop.Check(showerCars.SelectedCarId))
        {
            boughtUI.gameObject.SetActive(true);
            toBuyUI.gameObject.SetActive(false);
        }
        else
        {
            priceShower.SetValues(null, Game.Config.statsConfig.GetCarPrice(showerCars.SelectedCarId).ToString());
            boughtUI.gameObject.SetActive(false);
            toBuyUI.gameObject.SetActive(true);
        }
    }

    public void BuyCar()
    {
        if (shop.TryBuy(showerCars.SelectedCarId))
        {
            SetUICondition();
        }
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
}
