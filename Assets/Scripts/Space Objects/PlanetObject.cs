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

            //note: this might be inefficient coding, but it is readable
            if (collider.gameObject.GetComponent<AsteroidObject>().isPiercer)
            {
                gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
                gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
                StartCoroutine(Explode());
                //Destroy(collider.gameObject); //dont destroy it for a piercer
                collider.gameObject.GetComponent<AsteroidObject>().isPiercer = false; //instead disable the piercer ability
                Instantiate(resourceCloud, transform.position, transform.rotation);
            }
            else if (collider.gameObject.GetComponent<AsteroidObject>().isPusher)
            {
                GetComponent<Rigidbody2D>().velocity = collider.gameObject.GetComponent<Rigidbody2D>().velocity / 2;
                Destroy(collider.gameObject);
            }
            else if (collider.gameObject.GetComponent<AsteroidObject>().isDirectionalBlast)
            {
                Debug.Log("hit with blaster");
                gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
                gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
                StartCoroutine(Explode());
                Vector2 vel = collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                Destroy(collider.gameObject);
                var cloud = Instantiate(resourceCloud, transform.position, transform.rotation);
                cloud.GetComponent<Rigidbody2D>().velocity = vel / 2;
            }
            else
            {
                gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
                gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
                Instantiate(resourceCloud, transform.position, transform.rotation);
                StartCoroutine(Explode());
                Destroy(collider.gameObject);
            }
            
        }

        if(collider.gameObject.GetComponent<CometObject>())
        {
            collider.gameObject.GetComponent<CometObject>().LockAndDelete();
        }

        if (collider.gameObject.GetComponent<PreviewAsteroidObject>())
        {
            gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
            // did I say destroy collider here or not?
        }

    }

    protected IEnumerator Explode()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        if (transform.childCount > 0)
        {
            Debug.Log("Disabling Child");
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
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
