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
        adReward.text = "+"+Game.Config.AdReward.ToString();
        Game.Player.wallet.OnCurrencyChanged += UpdateValue;
        moneyValue.text = Game.Player.wallet.SoftCurrency.ToString();
        Localization.OnLanguageChange += Refresh;
    }

    private void OnDisable()
    {
        Game.Player.wallet.OnCurrencyChanged -= UpdateValue;
    }

    private void Update()
    {
#if UNITY_EDITOR
        moneyValueRect.gameObject.SetActive(true);
#endif
#if !UNITY_EDITOR
        moneyValueRect.gameObject.SetActive(GP_Ads.IsRewardedAvailable());
#endif

    }

    public void Refresh()
    {
        adReward.text = "+" + Game.Config.AdReward.ToString();
        Localization.OnLanguageChange -= Refresh;
    }

    private void UpdateValue() => moneyValue.text = Game.Player.wallet.SoftCurrency.ToString();

    public void AddMoney()
    {
        var instance = Instantiate(Game.Config.views.GetAdLoader);

        GP_Ads.ShowRewarded("",
        stringData =>
        {
            Game.Player.wallet.EarnSoft(Game.Config.AdReward);
        },
        () =>
        {
            moneyValueRect.gameObject.SetActive(false);
            instance.gameObject.SetActive(true);
        },
        isTrue => 
        { 
            Destroy(instance.gameObject);
        });
    }
}
