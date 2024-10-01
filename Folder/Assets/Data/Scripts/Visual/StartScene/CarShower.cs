using System.Collections.Generic;
using UnityEngine;

public class CarShower : MonoBehaviour
{
    [SerializeField] private Transform carSpawnPoint;
    private Dictionary<string, GameObject> carsTransforms = new Dictionary<string, GameObject>();
    private GameConfig.Car[] Cars => Game.Config.GetCars;
    public string SelectedCarId => Cars[currentCarId].Id;
    private int currentCarId = 0;
    
    void Start()
    {
        foreach (var car in Cars)
        {
            var instance = Instantiate(car.CardModel, carSpawnPoint);
            carsTransforms.Add(car.Id, instance);
            instance.SetActive(false);
        }
        currentCarId = Game.Player.settings.LastChoosedCar;
        if(currentCarId == -1) 
            currentCarId = 0;
        carsTransforms[Cars[currentCarId].Id].SetActive(true);
        var instanceView = Instantiate(Game.Config.views.GetSelectCarView);
        instanceView.Init(this);
        instanceView.OnNextCar += NextCarChoose;
        instanceView.OnPreviousCar += PreviousCarChoose;
    }

    public void NextCarChoose()
    {
        carsTransforms[Cars[currentCarId].Id].SetActive(false);
        currentCarId++;
        if (currentCarId >= Cars.Length) 
           currentCarId = 0; 
        Game.Player.settings.SetLastChoosedCar(currentCarId);
        carsTransforms[Cars[currentCarId].Id].SetActive(true);

    }

    public void PreviousCarChoose()
    {
        carsTransforms[Cars[currentCarId].Id].SetActive(false);
        currentCarId--;
        if (currentCarId < 0)
            currentCarId = Cars.Length - 1;
        Game.Player.settings.SetLastChoosedCar(currentCarId);
        carsTransforms[Cars[currentCarId].Id].SetActive(true);
    }
}
