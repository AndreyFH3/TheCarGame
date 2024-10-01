using GamePush;
using System.Collections.Generic;
using UnityEngine;

public class AdBlockView : MonoBehaviour
{
    [SerializeField] private List<RectTransform> transforms;
    private void Awake()
    {
        transforms.ForEach(x => x.gameObject.SetActive(false));
        if (GP_Ads.IsAdblockEnabled())
        {

        }        
    }

    public void Show()
    {
        transforms.ForEach(x => x.gameObject.SetActive(true));
    }

    public void Close()
    {
        transforms.ForEach(x => x.gameObject.SetActive(false));
    }

}
