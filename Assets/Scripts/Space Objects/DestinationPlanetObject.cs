using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestinationPlanetObject : MonoBehaviour
{
    public int[] resourcesNeeded;
    bool winning = false;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // if it's a big rock, womp womp.
        if(collider.GetComponent<AsteroidObject>())
        {
            Debug.Log("Womp Womp -- you destroyed your client!");
        }
        if(collider.GetComponent<CometObject>())
        {
            Debug.Log("Attempting to deliver resources from comet to target planet...");
            if(IsEnoughResources(collider.GetComponent<CometObject>().resources))
            {
                Victory();
                Debug.Log("Resources supplied -- LEVEL WON!!!");
            }
            else
            {
                Debug.Log("The comet didn't have the required materials.");
            }
        }
    }

    bool IsEnoughResources(int[] cometResources)
    {
        if (cometResources.Length != resourcesNeeded.Length) Debug.LogWarning("DestinationPlanet and Comet resource lists are of varying lengths!");
        for (int i = 0; i < cometResources.Length; i++) if (cometResources[i] < resourcesNeeded[i]) return false;
        return true;
    }

    void Victory()
    {
        GetComponent<Rotator>().enabled = false;
        GetComponentInChildren<Animator>().Play("Win");
        StartCoroutine(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Victory());
    }
}
