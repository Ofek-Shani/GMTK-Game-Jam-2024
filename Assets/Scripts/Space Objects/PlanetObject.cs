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

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        print("hi does this actually work?");
        print(collider);
        //Console.Write(collider);
        if (collider.GetComponent<AsteroidObject>() != null)
        {
            Destroy(gameObject);
            Instantiate(resourceCloud, transform.position, transform.rotation);

        }

    }


}
