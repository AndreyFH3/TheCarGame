using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongWayUIComponent : RaceUIComponent
{
    [SerializeField] private RectTransform showablePart;
    public override void Init(RaceController controller)
    {
        controller.OnPlayerWrongWay += Show;
        Show(false);
    }

    protected void Show(bool needShow)
    {
        showablePart.gameObject.SetActive(needShow);
    }
}
