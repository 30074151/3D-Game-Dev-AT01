using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    public GameObject gameWin;

    private void OnTriggerEnter(Collider other)
    {
        gameWin.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
