using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeypad : MonoBehaviour
{
    private KeypadInput keypadInput;
    [SerializeField] private MeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            keypadInput.canInteract = true;
        meshRenderer.material.SetFloat("_EmissiveIntensity", 10);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            keypadInput.canInteract = false;
    }
}
