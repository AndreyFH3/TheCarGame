using TMPro;
using UnityEngine;

public class LapsUIComponent : RaceUIComponent
{
    [SerializeField] TextMeshProUGUI lapsText;
    private CircleRaceController controllerCircle;
    private DriftRaceController controllerDrift;
    public override void Init(RaceController controller)
    {
        if(controller is not null)
        {
            if (controller is CircleRaceController raceController)
            {
                this.controllerCircle = raceController;
                this.controllerCircle.OnLapPassed += CircleRace;
                CircleRace(1);
            }
            else if (controller is DriftRaceController raceController2)
            {
                this.controllerDrift = raceController2;
                this.controllerDrift.OnLapPassed += DriftRace;
                DriftRace(1);

            }
        }
    }

    private void CircleRace(int lap)
    {
        lapsText.text = $"{Localization.Get("Lap")}: {lap}/{controllerCircle.Laps}";
    }
    private void DriftRace(int lap)
    {
        lapsText.text = $"{Localization.Get("Lap")}: {lap}/{controllerDrift.Laps}";
    }
}
