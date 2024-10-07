using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuObject;
    [SerializeField] Image staminaBar;

    private void OnEnable()
    {
        EventManager.pauseGameEvent += TogglePauseMenu;
    }

    private void OnDestroy()
    {
        EventManager.pauseGameEvent -= TogglePauseMenu;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TogglePauseMenu(bool toggle)
    {
        pauseMenuObject.SetActive(toggle);
    }

    public void ResumeGame()
    {
        EventManager.pauseGameEvent(false);
    }

    public void setStamina(float staminaAmount)
    {
        staminaBar.fillAmount = staminaAmount;
    }



    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
