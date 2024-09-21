using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DriftPointsShowComponent : RaceUIComponent
{
    [SerializeField] private TextMeshProUGUI pointsText;
    private DriftRaceController controller;
    public override void Init(RaceController controller)
    {
        this.controller = (DriftRaceController)controller;
        this.controller.OnDrift += ShowPoints;
        ShowPoints(0);
    }

    private void ShowPoints(float points)
    {
        pointsText.text = $"{Localization.Get("Points")}: {Mathf.RoundToInt(points)}";
    }
}
