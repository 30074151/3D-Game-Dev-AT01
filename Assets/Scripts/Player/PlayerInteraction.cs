using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //the maximum interaction distance
    [SerializeField] private float iDistance;

    public bool gamePaused;


    private void OnEnable()
    {
        EventManager.pauseGameEvent += TogglePaused;
    }

    private void OnDestroy()
    {
        EventManager.pauseGameEvent -= TogglePaused;
    }

    private void Update()
    {
        if (!gamePaused)
        {
            //check for player left click
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                //check for interactable in from of camera (within interaction distance
                if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, iDistance))
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    return;
                }   //if no - do nothing
                    //if yes - call activate
                IInteraction interaction;
                if (hit.collider.gameObject.TryGetComponent<IInteraction>(out interaction))
                {
                    interaction.Activate();
                }
                
            }
        }
    }

    private void TogglePaused(bool toggle)
    {
        gamePaused = toggle;
    }
}
