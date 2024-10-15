using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject GameoverScreen;
    [SerializeField] GameObject waypoint;
    [SerializeField] bool playerDead = false;
    [SerializeField] CharacterController cTroller;

    private void Awake()
    {
        cTroller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }
    public void KillPlayer()
    {
        playerDead = true;
        cTroller.enabled = false;
        GameoverScreen.SetActive(playerDead);
        waypoint.SetActive(false);
        EventManager.pauseGameEvent(playerDead);
    }
}
