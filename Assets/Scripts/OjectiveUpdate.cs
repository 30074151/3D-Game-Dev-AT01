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
        //update objective text method subscribe to updateobjective event
    }

    //this method should take in string and update objective text to display string
    private void UpdateObjectiveText(int doorID)
    {
        objectiveText.text = "Get to the Office";
    }

    private void OnDestroy()
    {
        EventManager.unlockDoorEvent -= UpdateObjectiveText;
    }

}
