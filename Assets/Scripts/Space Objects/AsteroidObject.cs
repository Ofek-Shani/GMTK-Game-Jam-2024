using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidObject : MonoBehaviour
{
    public bool isPusher = false;
    public bool isPiercer = false;

    public bool isDirectionalBlast = false;
    Animator anim;

    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.AddSpacePhysicsObject(GetComponent<SpacePhysics>());
        anim = GetComponent<Animator>();
    }

    
}
