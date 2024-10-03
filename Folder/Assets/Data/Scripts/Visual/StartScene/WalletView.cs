using GamePush;
using TMPro;
using UnityEngine;


public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyValue;
    [SerializeField] private RectTransform moneyValueRect;
    [SerializeField] private TextMeshProUGUI adReward;

    private void OnEnable()
    {
        adReward.text = Localization.Get("AdReward", Game.Config.AdReward);
        Game.Player.wallet.OnCurrencyChanged += UpdateValue;
        moneyValue.text = Game.Player.wallet.SoftCurrency.ToString();
        Localization.OnLanguageChange += Refresh;
    }

    private void OnDisable()
    {
        Game.Player.wallet.OnCurrencyChanged -= UpdateValue;
    }
    
    public void Refresh()
    {
        adReward.text = Localization.Get("AdReward", Game.Config.AdReward);
        Localization.OnLanguageChange -= Refresh;
    }

    private void UpdateValue() => moneyValue.text = Game.Player.wallet.SoftCurrency.ToString();

    public void AddMoney()
    {
        GP_Ads.ShowRewarded("",
        stringData =>
        {
            Game.Player.wallet.EarnSoft(Game.Config.AdReward);
        },
        () =>
        {
            moneyValueRect.gameObject.SetActive(false);
        },
        isTrue => { });
    }
}
