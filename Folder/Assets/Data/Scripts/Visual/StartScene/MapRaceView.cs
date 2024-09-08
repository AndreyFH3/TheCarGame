using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRaceView : MonoBehaviour
{
    [SerializeField] private Image mapImage;

    public void SetMapImage(Sprite sprite)
    {
        mapImage.sprite = sprite;
    }
}