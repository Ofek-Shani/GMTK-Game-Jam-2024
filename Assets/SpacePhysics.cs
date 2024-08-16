using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePhysics : MonoBehaviour
{
    // GLOBAL PHYSICS CONSTANTS
    const float UNIVERSAL_GRAVITY_CONSTANT = 1;

    // PER OBJECT CONFIGS
    [SerializeField]
    public bool emitsGravity { get; private set; }
    public bool canMove { get; private set; }
    [SerializeField]
    public float mass { get; private set; }

    // Internal Components
    Rigidbody2D rb;

    // Other things for internal physics logic
    List<SpacePhysics> otherBodies;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] otherPhysics = GameObject.FindGameObjectsWithTag("GravityEmitter");
        foreach (GameObject g in otherPhysics) otherBodies.Add(g.GetComponent<SpacePhysics>());
    }

    /// <summary>
    /// Gets the net force applied on this object by all other gravity emitters and uses Rigidbody to apply it.
    /// </summary>
    void DoPhysics()
    {
        Vector3 netForce = Vector3.zero;
        foreach(SpacePhysics sp in otherBodies)
        {
            netForce += GetForce(sp);
        }
        rb.AddForce(netForce);
    }

    
    /// <summary>
    /// Gets the force exerted by this object onto the target gameObject using Universal Gravity Formula. REQUIRES target has SpacePhysics component.
    /// </summary>
    /// <param name="target"></param>
    Vector3 GetForce(SpacePhysics target)
    {
        if (!emitsGravity) return Vector3.zero;

        // get all the values for the formula
        float r = (transform.position - target.transform.position).magnitude;
        Vector3 rHat = (transform.position - target.transform.position).normalized;
        float m1 = this.mass;
        float m2 = target.mass;
        // use the formula
        float vecMag = UNIVERSAL_GRAVITY_CONSTANT * m1 * m2 / Mathf.Pow(r, 2);
        return rHat * vecMag;

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) DoPhysics();
    }
}
