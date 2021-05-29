using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private GameObject currentTarget;

    [SerializeField] private float doorCloseSpeed = 35f;
    [SerializeField] private float doorOpenSpeed = 35f;
    [SerializeField] private float interactionRange = 50f;

    [SerializeField] private AudioClip[] wrongDoorClips;
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorCloseClip;

    [SerializeField] private Animator wallAnim;
    [SerializeField] private Animator lightAnim;
    private AudioSource source;

    private bool startSpin = false;
    private bool doorClosed = false;
    private bool correctDoor = false;
    private bool doorOpening = false;
    private bool canInteractWithDoor = false;
    private bool spinComplete = false;
    private bool doorFinishedOpening = false;

    private int highlightLayer;
    private int doorLayer;

    private Vector3 screenCenter;

    private FlashlightFlicker flicker;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        highlightLayer = LayerMask.NameToLayer("Highlight");
        doorLayer = LayerMask.NameToLayer("Doors");
        screenCenter = new Vector3(Screen.width >> 1, Screen.height >> 1);
        flicker = FindObjectOfType<FlashlightFlicker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !spinComplete)
        {
            startSpin = true;
            spinComplete = true;
        }
    }

    private void Update()
    {
        PlayAnimation();
        InteractWithDoor();
        HighlightDoors();
        OpenDoor();
    }

    void PlayAnimation()
    {
        if (startSpin)
        {
            Debug.Log(Quaternion.Angle(door.transform.localRotation, Quaternion.Euler(0, -90, 0)));
            if (!doorClosed)
                door.transform.Rotate(-Vector3.up * doorCloseSpeed * Time.deltaTime);
            if (Quaternion.Angle(door.transform.localRotation, Quaternion.Euler(0, -90, 0)) < 1f)
            {
                doorClosed = true;
                source.clip = doorCloseClip;
                source.Play();
                wallAnim.SetBool("Spin", true);
                lightAnim.SetBool("playAnim", true);
                StartCoroutine(flicker.CarouselFlicker());
                startSpin = false;
            }
        }
    }

    void HighlightDoors()
    {
        if (doorClosed)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out RaycastHit hit, interactionRange, LayerMask.GetMask("Doors", "Highlight")))
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.yellow);
                GameObject target = hit.collider.gameObject;

                if (currentTarget != target)
                {
                    currentTarget = target;
                    currentTarget.layer = highlightLayer;
                    canInteractWithDoor = true;
                    if (currentTarget.CompareTag("Exit Door"))
                    {
                        correctDoor = true;
                    }
                    else
                    {
                        correctDoor = false;
                    }
                    Debug.Log(correctDoor);
                }
            }
            else if (currentTarget != null)
            {
                currentTarget.layer = doorLayer;
                currentTarget = null;
                canInteractWithDoor = false;
                correctDoor = false;
            }
        }
    }

    void InteractWithDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canInteractWithDoor)
            {
                if (!correctDoor)
                {
                    int i = Random.Range(0, wrongDoorClips.Length);
                    source.clip = wrongDoorClips[i];
                    source.Play();
                }
                else
                {
                    doorOpening = true;
                }
            }
        }
    }

    void OpenDoor()
    {
        if (doorOpening && !doorFinishedOpening)
        {
            door.transform.Rotate(Vector3.up * doorOpenSpeed * Time.deltaTime);
            source.clip = doorOpenClip;
            source.Play();
            if (Quaternion.Angle(door.transform.localRotation, Quaternion.Euler(0, 0, 0)) <= .3f)
            {
                doorOpening = false;
                doorClosed = false;
                doorFinishedOpening = true;
            }
        }
        
    }
}
