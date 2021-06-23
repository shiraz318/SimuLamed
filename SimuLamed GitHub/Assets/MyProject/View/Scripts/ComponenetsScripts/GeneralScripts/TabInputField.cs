using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    private List<GameObject> inputList; // list of all input fields in the scene.
    EventSystem system;
    void Start()
    {
        system = EventSystem.current;
        // Initialize the list.
        inputList = new List<GameObject>();
        InputField[] array = transform.GetComponentsInChildren<InputField>();
        for (int i = 0; i < array.Length; i++)
        {
            inputList.Add(array[i].gameObject);
        }
    }
    void Update()
    {
        // Check if TAB is clicked.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // If any input field is clicked.
            if (inputList.Contains(system.currentSelectedGameObject))
            {
                //Positive order.
                GameObject next = NextInput(system.currentSelectedGameObject);
                system.SetSelectedGameObject(next);
             
            }
        }
    }

    // Get the next object.
    private GameObject NextInput(GameObject input)
    {
        int indexNow = IndexNow(input);
        if (indexNow + 1 < inputList.Count)
        {
            return inputList[indexNow + 1].gameObject;
        }
        else
        {
            return inputList[0].gameObject;
        }
    }

    // Get the sequence of the currently selected object.
    private int IndexNow(GameObject input)
    {
        int indexNow = 0;
        for (int i = 0; i < inputList.Count; i++)
        {
            if (input == inputList[i])
            {
                indexNow = i;
                break;
            }
        }
        return indexNow;
    }

    
}


