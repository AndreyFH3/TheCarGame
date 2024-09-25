using GamePush;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyValue;
    [SerializeField] private TextMeshProUGUI adReward;

    private void OnEnable()
    {
        adReward.text = Localization.Get("AdReward", Game.Config.AdReward);
        Game.Player.wallet.OnCurrencyChanged += UpdateValue;
        moneyValue.text = Game.Player.wallet.SoftCurrency.ToString();
    }

    private void OnDisable()
    {
        Game.Player.wallet.OnCurrencyChanged -= UpdateValue;
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
            gameObject.SetActive(false);
        },
        isTrue => { });
    }
}
