using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OjectiveUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.unlockDoorEvent += UpdateObjectiveText;
    }

    private void UpdateObjectiveText(int doorID)
    {
        objectiveText.text = "Find the door";
    }

    private void OnDestroy()
    {
        EventManager.unlockDoorEvent -= UpdateObjectiveText;
    }

}
