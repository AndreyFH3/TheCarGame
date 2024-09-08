using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrackPart : MonoBehaviour
{
    [SerializeField] private string id;
    public string Id => id;
    public SavableTransform GetSaveData()
    {
        return new SavableTransform(transform.position, transform.localScale, transform.eulerAngles, Id);
    }

    public void SetSavedData(SavableTransform savable)
    {
        transform.position = savable.Position;
        transform.localScale = savable.Scale;
        transform.eulerAngles = savable.RotationEuler;
    }
}
[System.Serializable]
public class TrackFullInfo
{
    [SerializeField] private List<CollidersCheckers> colliders;
    [SerializeField] private List<SavableTransform> transforms;

    public TrackFullInfo(List<CollidersCheckers> colliders, List<SavableTransform> transforms)
    {
        this.colliders = colliders;
        this.transforms = transforms;
    }

    public CollidersCheckers[] GetColliders => colliders.ToArray();
    public SavableTransform[] GetTransforms => transforms.ToArray();
}


[System.Serializable]
public class CollidersCheckers : SavableTransform
{
    [SerializeField] private int WayNumber;
    public int WayIndex => WayNumber;
    public CollidersCheckers(Vector3 position, Vector3 scale, Vector3 rotationEuler, int number) : base(position, scale, rotationEuler)
    {
        WayNumber = number;
        id = number.ToString();
    }
}

[System.Serializable]
public class SavableTransform
{
    [SerializeField] protected string id;
    [SerializeField] protected Vector3 position;
    [SerializeField] protected Vector3 scale;
    [SerializeField] protected Vector3 rotationEuler;

    public SavableTransform(Vector3 position, Vector3 scale, Vector3 rotationEuler, string id)
    {
        this.id = id;
        this.position = position;
        this.scale = scale;
        this.rotationEuler= rotationEuler;
    }

    protected SavableTransform(Vector3 position, Vector3 scale, Vector3 rotationEuler)
    {
        this.position = position;
        this.scale = scale;
        this.rotationEuler = rotationEuler;
    }
    public string Id => id;
    public Vector3 Position => position;
    public Vector3 Scale => scale;
    public Vector3 RotationEuler => rotationEuler;
}