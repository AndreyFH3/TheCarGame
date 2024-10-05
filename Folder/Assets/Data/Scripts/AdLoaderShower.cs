using GamePush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdLoaderShower : MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    private void Update()
    {
        rect.Rotate(new Vector3(0,0,1), 60 * Time.deltaTime);
    }
}
