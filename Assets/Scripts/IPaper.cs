using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPaper : MonoBehaviour, IInteraction
{
    public GameObject gameWinTrigger;
    public void Activate()
    {
        //use updateobjective event to send new objective
        Debug.Log("Picked up document");

        EventManager.waypointUpdateEvent(1);

        gameObject.SetActive(false);
        gameWinTrigger.SetActive(true);
    }
}
