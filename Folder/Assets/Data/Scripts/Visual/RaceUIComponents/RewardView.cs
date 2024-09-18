using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetValues(Sprite icon, string text)
    {
        if(icon is not null)
            this.icon.sprite = icon;
        amountText.text = text;
    }
}
