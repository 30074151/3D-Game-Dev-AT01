using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKey : MonoBehaviour, IInteraction
{
    public enum KeyCode { OfficeKey }
    public KeyCode keyCode = KeyCode.OfficeKey;

    public void Activate()
    {
        //call the 'picked up key' event and pass it our keycode value
        EventManager.unlockDoorEvent((int)keyCode);
        //use updateobjective event to send new objective
        Debug.Log("Picked up key");

        EventManager.waypointUpdateEvent(1);

        gameObject.SetActive(false);
    }

}
