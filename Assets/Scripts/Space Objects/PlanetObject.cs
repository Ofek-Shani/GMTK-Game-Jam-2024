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


    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
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
                Destroy(gameObject);
                //Destroy(collider.gameObject); //dont destroy it for a piercer
                collider.gameObject.GetComponent<AsteroidObject>().isPiercer = false; //instead disable the piercer ability
                Instantiate(resourceCloud, transform.position, transform.rotation);
            }
            else if (collider.gameObject.GetComponent<AsteroidObject>().isPusher)
            {
                GetComponent<Rigidbody2D>().velocity = collider.gameObject.GetComponent<Rigidbody2D>().velocity / 20;
                Destroy(collider.gameObject);
            }
            else if (collider.gameObject.GetComponent<AsteroidObject>().isDirectionalBlast)
            {
                gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
                gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
                Destroy(gameObject);
                Vector2 vel = collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                Destroy(collider.gameObject);
                var cloud = Instantiate(resourceCloud, transform.position, transform.rotation);
                cloud.GetComponent<Rigidbody2D>().velocity = vel / 20;
            }
            else
            {
                gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
                gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
                Destroy(gameObject);
                Destroy(collider.gameObject);
                Instantiate(resourceCloud, transform.position, transform.rotation);
            }
            
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

    //Debug Function
    string PrintList(List<GameObject> toPrint)
    {
        string strToPrint = "";
        foreach (GameObject g in toPrint) strToPrint += g.name + " ";
        return strToPrint;
    }


}
