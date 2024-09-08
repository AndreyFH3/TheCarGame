using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UpgradeCardView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private List<Image> upgradeShower;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI priceShower;
    [SerializeField] private Image buttonUpgade;
    private CarCharacteristic characteristic;
    public void Init(CarCharacteristic carIdType)
    {
        characteristic = carIdType;
        icon.sprite = Game.Config.icons.GetSpriteCharacteristic(carIdType.Type);
        cardName.text = carIdType.Type.ToString();
        SetUpgrades();
    }

    private void SetUpgrades()
    {
        buttonUpgade.gameObject.SetActive(!characteristic.IsMaxUpgrade);

        for (int i = 0; i < upgradeShower.Count; i++)
        {
            upgradeShower[i].sprite = Game.Config.icons.GetUpgraded(characteristic.Level - 1 >= i);
        }
        priceShower.text = characteristic.Cost.ToString();
    }

    public void UpgradeCharacteristic() 
    {
        if (Game.Player.wallet.CheckSoftEnough(0))
        {
            characteristic.Upgrade();
            SetUpgrades();
        }
    }
}
