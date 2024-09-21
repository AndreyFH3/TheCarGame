using TMPro;
using UnityEngine;


public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyValue;

    private void OnEnable()
    {
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
        Game.Player.wallet.EarnSoft(500);
    }
}
