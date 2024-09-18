using NaughtyAttributes;
using UnityEngine;

public class TreeRotator : MonoBehaviour
{
    [SerializeField] private float min;
    [SerializeField] private float max;

    [Button]
    public void Rotate()
    {
        foreach (Transform t in transform)
        {
            t.eulerAngles = new Vector3(0, Random.Range(min,max), 0);
        }
    }
}