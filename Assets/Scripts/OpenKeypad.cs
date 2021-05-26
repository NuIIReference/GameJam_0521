using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeypad : MonoBehaviour
{
    private KeypadInput keypadInput;
    private int highlightLayer;
    private int defaultLayer;

    private void Start()
    {
        defaultLayer = LayerMask.NameToLayer("Default");
        highlightLayer = LayerMask.NameToLayer("Highlight");
        keypadInput = GetComponent<KeypadInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.layer = highlightLayer;
            keypadInput.canInteract = true;
            Debug.Log("Found Player");
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.layer = defaultLayer;
            keypadInput.canInteract = false;
            keypadInput.isActive = false;
        }
    }
}
