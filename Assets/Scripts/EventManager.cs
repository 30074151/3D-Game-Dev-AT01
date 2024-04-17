using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("You can only have one event manager in this scene");
            gameObject.SetActive(false);
        }
    }

    public delegate void UnlockDoor(int id);
    public static UnlockDoor unlockDoorEvent;

    public delegate void PauseGame(bool toggle);
    public static PauseGame pauseGameEvent;
    //update objective event (take in a string)***
    //event updateobjectivemarker***
}
