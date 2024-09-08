using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputView : MonoBehaviour
{
    [SerializeField] private PrometeoTouchInput forward;
    [SerializeField] private PrometeoTouchInput back;
    [SerializeField] private PrometeoTouchInput left;
    [SerializeField] private PrometeoTouchInput right;
    [SerializeField] private PrometeoTouchInput brakes;

    public GameObject Forward => forward.gameObject;
    public GameObject Back => back.gameObject;
    public GameObject Left => left.gameObject;
    public GameObject Right => right.gameObject;
    public GameObject Brakes => brakes.gameObject;
}
