using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OjectiveUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.waypointUpdateEvent += UpdateObjectiveIndex;
        //update objective text method subscribe to updateobjective event
    }

    private void Update()
    {
        if(index == 0)
        {
            objectiveText.text = "Get the Office Key";
        }
        if(index == 1)
        {
            objectiveText.text = "Head to the Main Office";
        }
        if(index == 2)
        {
            objectiveText.text = "Take the Evidence Document";
        }
        if(index == 3)
        {
            objectiveText.text = "Exit the Asylum";
        }
    }
    //this method should take in string and update objective text to display string
    private void UpdateObjectiveIndex(int number)
    {
        index += number;
    }

    private void OnDestroy()
    {
        EventManager.waypointUpdateEvent -= UpdateObjectiveIndex;
    }

}
