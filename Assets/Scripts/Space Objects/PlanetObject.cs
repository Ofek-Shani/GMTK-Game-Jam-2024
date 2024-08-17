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


    // Start is called before the first frame update
    void Start()
    {
        
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
            Destroy(gameObject);
            Destroy(collider.gameObject);
            Instantiate(resourceCloud, transform.position, transform.rotation);

        }

        if(collider.gameObject.GetComponent<CometObject>())
        {
            Destroy(collider.gameObject);
        }

    }


}
