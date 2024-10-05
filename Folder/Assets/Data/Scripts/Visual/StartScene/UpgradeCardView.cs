using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UpgradeCardView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private RewardView price;
    [SerializeField] private List<Image> upgradeShower;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Button buttonUpgade;
    private CarCharacteristic characteristic;
    public void Init(CarCharacteristic carIdType)
    {
        characteristic = carIdType;
        if(characteristic is null)
        {
            buttonUpgade.gameObject.SetActive(false);
            price.gameObject.SetActive(false);  
            return;
        }
        UpdateName();
        icon.sprite = Game.Config.icons.GetSpriteCharacteristic(carIdType.Type);
        SetUpgrades();
        Game.Player.wallet.OnCurrencyChanged += CheckBuyAvailibleByMoney;
    }

    private void UpdateName()
    {
        cardName.text = Localization.Get(characteristic.Type.ToString());

    }

    private void OnEnable()
    {
        Localization.OnLanguageChange += UpdateName;
    }

    private void OnDisable()
    {
        Localization.OnLanguageChange -= UpdateName;
    }

    private void CheckBuyAvailibleByMoney()
    {
        buttonUpgade.interactable = characteristic.CanUpgrade;
    }

    private void SetUpgrades()
    {
        buttonUpgade.gameObject.SetActive(!characteristic.IsMaxUpgrade);
        price.gameObject.SetActive(!characteristic.IsMaxUpgrade);

        for (int i = 0; i < upgradeShower.Count; i++)
        {
            upgradeShower[i].sprite = Game.Config.icons.GetUpgraded(characteristic.Level - 1 >= i);
        }
        price.SetValues(null, characteristic.Cost.ToString());
    }

    private void OnDestroy()
    {
        Game.Player.wallet.OnCurrencyChanged -= CheckBuyAvailibleByMoney;
    }

    public void UpgradeCharacteristic()
    {
        characteristic.Upgrade();
        SetUpgrades();
    }
}