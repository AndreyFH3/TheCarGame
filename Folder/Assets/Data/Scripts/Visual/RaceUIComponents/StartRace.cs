using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class StartRace : RaceUIComponent
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int seconds;
    private RaceController raceController;
    public override void Init(RaceController controller)
    {
        raceController = controller;
        StartCoroutine(WaitStartRace());
    }

    private IEnumerator WaitStartRace()
    {
        yield return new WaitForSecondsRealtime(.2f);
        raceController.SetPaused();
        while (seconds > 0)
        {
            textMesh.text = seconds.ToString();
            seconds--;
            Debug.Log($"{seconds}");
            yield return new WaitForSecondsRealtime(1);
        }
        raceController.SetUnpaused();
        raceController.StartRace();
        gameObject.SetActive(false);
    }
}
