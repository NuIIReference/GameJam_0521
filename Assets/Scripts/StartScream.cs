using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScream : MonoBehaviour
{
    private AudioSource source;
    public float playDelay = 5f;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.PlayDelayed(playDelay);
    }

    
}
