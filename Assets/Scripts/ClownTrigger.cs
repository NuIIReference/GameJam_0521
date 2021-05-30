using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownTrigger : MonoBehaviour
{
    [SerializeField] private Animator clownToAnimate;
    private bool played = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !played)
        {
            clownToAnimate.SetBool("playAnim", true);
            StartCoroutine(DisableObject(2f));
            played = true;
        }
    }

    IEnumerator DisableObject(float time)
    {
        yield return new WaitForSeconds(time);
        clownToAnimate.gameObject.SetActive(false);
    }
}
