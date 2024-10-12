 using UnityEngine;

public class TrackWay : MonoBehaviour
{
    [SerializeField] private int numberWay;
    public int NumberWay => numberWay;

    public System.Action<int> OnColliderTouched;
    public System.Action<int> OnTouchColliderCalculate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent(out PrometeoCarController car))
        {
            OnColliderTouched?.Invoke(NumberWay);
            OnTouchColliderCalculate?.Invoke(NumberWay);
        }
    }

    public CollidersCheckers GetSaveData()
    {
        return new CollidersCheckers(transform.position, transform.localScale, transform.eulerAngles, NumberWay);
    }

    public void SetSavedData(CollidersCheckers savable)
    {
        transform.position = savable.Position;
        transform.localScale = savable.Scale;
        transform.eulerAngles = savable.RotationEuler;
        numberWay = savable.WayIndex;
    }

}
