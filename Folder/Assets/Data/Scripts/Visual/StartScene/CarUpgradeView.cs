using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUpgradeView : MonoBehaviour
{
    [SerializeField] private UpgradeCardView card;
    [SerializeField] private Transform cardsParent;
    [SerializeField] private Transform targetLook;
    public System.Action OnClose;

    public void Init(string carId)
    {
        var characteristics = Game.Player.GetCarCharacteristics(carId);
        foreach (var characteristic in characteristics.Characteristics)
        { 
            var instance = Instantiate(card, cardsParent);
            instance.Init(characteristic);
        }
        card.gameObject.SetActive(false);
    }

    public void Close()
    {
        OnClose?.Invoke();
        Destroy(gameObject);
    }


}
