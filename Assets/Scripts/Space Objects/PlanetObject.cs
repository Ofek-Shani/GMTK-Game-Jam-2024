using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlanetObject : MonoBehaviour
{
    public ResourceCloudObject resourceCloud;
    public UInt64 population;

    const float TIME_FROM_EXPLODE_TO_DESTROY = 1f;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Pause();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.AddSpacePhysicsObject(GetComponent<SpacePhysics>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) 
    {
        //Debug.Log("hi does this actually work?");
        //Debug.Log(collider);
        //Console.Write(collider);
        if (collider.gameObject.GetComponent<AsteroidObject>() != null)
        {
            gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
            gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
            
            Destroy(collider.gameObject);
            Instantiate(resourceCloud, transform.position, transform.rotation);
            StartCoroutine(Explode());
        }

        if(collider.gameObject.GetComponent<CometObject>())
        {
            gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.GetComponent<PreviewAsteroidObject>())
        {
            gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
            // did I say destroy collider here or not?
        }

    }

    IEnumerator Explode()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(TIME_FROM_EXPLODE_TO_DESTROY);
        Destroy(gameObject);
    }

    //Debug Function
    string PrintList(List<GameObject> toPrint)
    {
        string strToPrint = "";
        foreach (GameObject g in toPrint) strToPrint += g.name + " ";
        return strToPrint;
    }


}
