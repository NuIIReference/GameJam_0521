using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class KeypadInput : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    private InputField textInput;
    private string code = "1408";
    [HideInInspector] public bool isActive = false;
    [HideInInspector] public bool canInteract = false;
    [SerializeField] private GameObject doorToOpen;
    [SerializeField] private float doorOpenSpeed = 5f;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] toneClips;
    private bool openDoor;

    private void Start()
    {
        textInput = textBox.GetComponent<InputField>();
        textInput.contentType = InputField.ContentType.Pin;
        textBox.SetActive(false);
        InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
        submitEvent.AddListener(CompareString);
        //InputField.OnChangeEvent onChange = new InputField.OnChangeEvent();
        //onChange.AddListener(PlayKeyTone);
        textInput.onEndEdit = submitEvent;
    }

    private void Update()
    {
        GetInput();
        ActivateTextBox();
        OpenDoor();
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
            openDoor = true;
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

    void OpenDoor()
    {
        if (openDoor)
        {
            doorToOpen.transform.Rotate(Vector3.up * doorOpenSpeed * Time.deltaTime);
            if (doorToOpen.transform.localEulerAngles.y >= 90)
            {
                openDoor = false;
                //doorToOpen.transform.localRotation = Quaternion.Slerp(doorToOpen.transform.localRotation, Quaternion.Euler(Vector3.up * 90), doorOpenSpeed * Time.deltaTime);
            }
        }
            
    }

    public void PlayKeyTone()
    {
        int clip = UnityEngine.Random.Range(0, toneClips.Length);
        source.clip = toneClips[clip];
        source.Play(); 
    }
}
