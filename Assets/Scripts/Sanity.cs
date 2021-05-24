using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanity : MonoBehaviour
{
    public float maxSanity;
    public float currentSanity;
    public float damageOverTime;

    public bool takeSanityLoss;

    private void Start()
    {
        currentSanity = maxSanity;
    }

    private void Update()
    {
        SanityLoss();
    }

    void SanityLoss()
    {
        if (takeSanityLoss)
        {
            currentSanity -= damageOverTime * Time.deltaTime;
        }       
    }

    public void TakeSanityDamage(float damage)
    {
        currentSanity -= damage;
    }

    public void RestoreSanity(float amount)
    {
        currentSanity += amount;
    }

    public float SanityNormalized()
    {
        return currentSanity / maxSanity;
    }
}
