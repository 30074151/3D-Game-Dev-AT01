using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAlarm : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.unlockDoorEvent += PlayDoorAlarm;
    }

    private void PlayDoorAlarm(int doorID)
    {
        source.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        EventManager.unlockDoorEvent -= PlayDoorAlarm;
    }
}
