using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/ApplicationSettings", order = 3)]
public class ApplicationSettings : ScriptableObject
{
    [SerializeField] private float timeToSave = 1f;
    [SerializeField] private string baseScene;
    [SerializeField] private string raceScene;

    public float SaveInterval => timeToSave;
    public string Base => baseScene;
    public string Race => raceScene;
}
