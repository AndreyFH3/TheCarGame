using TMPro;
using UnityEngine;

public class SpeedometrUIComponent : RaceUIComponent
{
    [SerializeField] private TextMeshProUGUI speedText;
    public RaceController controller;

    public override void Init(RaceController controller)
    {
        this.controller = controller;
        controller.CarController.OnCarSpeedChage += UpdateText;
        UpdateText(0);

    }

    private void UpdateText(float speed)
    {
        if(!controller.Pause)
            speedText.text = speed.ToString() + "km/h";
    }
}
