using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
