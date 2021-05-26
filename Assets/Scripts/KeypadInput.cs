using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class KeypadInput : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private InputField textInput;
    private string code = "1408";
    [HideInInspector] public bool isActive = false;
    [HideInInspector] public bool canInteract = false;

    private void Start()
    {
        textInput.contentType = InputField.ContentType.Pin;
        textBox.SetActive(false);
        InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
        submitEvent.AddListener(CompareString);
        textInput.onEndEdit = submitEvent;
    }

    private void Update()
    {
        GetInput();
        ActivateTextBox();
    }

    void GetInput()
    {
        if (canInteract)
            if (Input.GetKeyDown(KeyCode.E))
                isActive = !isActive;
    }

    void ActivateTextBox()
    {
        if (isActive)
        {
            textBox.SetActive(true);
            EventSystem.current.SetSelectedGameObject(textBox);
        }
        else
        {
            textBox.SetActive(false);
        }
    }

    void CompareString(string input)
    {
        if (String.Equals(input, code))
        {
            isActive = false;
            //Open the Door
            Debug.Log("The code is right : " + input);
        }
        else
        {
            Debug.Log("The code is wrong : " + input);
            textInput.text = null;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(textBox);
            if (!textBox.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
