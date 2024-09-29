using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsConfig", menuName = "ScriptableObjects/Create Sounds Config", order = 1)]
public class SoundsConfig : ScriptableObject
{
    [SerializeField] private AudioClip menuSound;
    [SerializeField] private List<AudioClip> raceClips;

    public AudioClip MenuSound => menuSound;

    public AudioClip RaceSound
    {
        get
        {
            return raceClips[Random.Range(0, raceClips.Count)];
        }
    }

}
