using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] float sensitivity = 2.5f;

    [SerializeField] float drag = 1.5f;

    [SerializeField] bool lookEnable = true;

    private Transform character;
    private Vector2 mouseDir;
    private Vector2 smoothing;
    private Vector2 result;

    public bool gamePaused;


    public bool CursorToggle 
    { 
        set 
        { 
            if(value == true) 
            { 
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                gamePaused = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gamePaused = false;
            }
        } 
    }


    private void Awake()
    {
        character = transform.parent;
        CursorToggle = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gamePaused)
        {
            MouseInput();
        }
        

        if (Input.GetButtonDown("Pause"))
        {
            if(gamePaused == true)
            {
                EventManager.pauseGameEvent(false);
            }
            else
            {
                EventManager.pauseGameEvent(true);
            }
        }
    }

    private void OnEnable()
    {
        EventManager.pauseGameEvent += TogglePause;
    }

    private void OnDestroy()
    {
        EventManager.pauseGameEvent -= TogglePause;
    }

    private void TogglePause(bool toggle)
    {
        CursorToggle = toggle;
    }

    private void MouseInput()
    {
        if(lookEnable == true)
        {
            mouseDir = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity);

            smoothing = Vector2.Lerp(smoothing, mouseDir, 1 / drag);

            result += smoothing;
            result.y = Mathf.Clamp(result.y, -80, 80);

            transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right);
            character.rotation = Quaternion.AngleAxis(result.x, character.transform.up);
        }
    }

}
