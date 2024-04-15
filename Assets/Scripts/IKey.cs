using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKey : MonoBehaviour, IInteraction
{
    public enum KeyCode { OfficeKey, BlueKey, KeySix }
    public KeyCode keyCode = KeyCode.OfficeKey;

    public void Activate()
    {
        //call the 'picked up key' event and pass it our keycode value
        EventManager.unlockDoorEvent((int)keyCode);
        Debug.Log("Picked up key");

        gameObject.SetActive(false);
    }

}
