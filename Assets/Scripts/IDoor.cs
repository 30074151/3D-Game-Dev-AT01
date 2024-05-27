using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDoor : MonoBehaviour, IInteraction
{

    public bool locked = true;

    public enum DoorID {  OfficeDoor, BlueDoor, DoorSix }
    public DoorID doorID = DoorID.OfficeDoor;

    [SerializeField] Animator animator;

    bool doorOpen = false;

    private void Start()
    {
        EventManager.unlockDoorEvent += UnlockDoor;
    }

    private void OnDestroy()
    {
        EventManager.unlockDoorEvent -= UnlockDoor;
    }

    private void UnlockDoor(int id)
    {
        if(id == (int)doorID)
        {
            locked = false;
        }
    }

    public void Activate()
    {
        if (locked)
        {
            Debug.Log("You need the " + doorID + " key");
        }
        else
        {
            if (!doorOpen)
            {
                animator.Play("DoorOpen");
                doorOpen = true;
                Debug.Log("Door opens");
            }
            else
            {
                animator.Play("DoorClose");
                doorOpen = false;
                Debug.Log("Door closes");
            }
        }

        EventManager.waypointUpdateEvent(1);

    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyNPCMovement>(out EnemyNPCMovement enmpc))
        {
            animator.Play("DoorOpen");
            doorOpen = true;
            Debug.Log("Door opens");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyNPCMovement>(out EnemyNPCMovement enmpc))
        {
            animator.Play("DoorClose");
            doorOpen = false;
            Debug.Log("Door closes");
        }
    }
}
