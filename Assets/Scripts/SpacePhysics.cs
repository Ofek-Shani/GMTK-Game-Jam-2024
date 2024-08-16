using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePhysics : MonoBehaviour
{
    // GLOBAL PHYSICS CONSTANTS
    const float UNIVERSAL_GRAVITY_CONSTANT = 1f;

    // PER OBJECT CONFIGS
    
    public bool emitsGravity;
    public bool canMove;
    [Range(0.0f, 10.0f)]
    public float mass;

    // Internal Components
    Rigidbody2D rb;

    // Other things for internal physics logic
    List<SpacePhysics> otherBodies = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

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
        Vector2 netForce = Vector2.zero;
        foreach(SpacePhysics sp in otherBodies)
        {
            netForce += sp.GetForce(this);

            Debug.Log(netForce);
        }
        Debug.Log(netForce);
        rb.AddForce(netForce);
    }

    
    /// <summary>
    /// Gets the force exerted by this object onto the target gameObject using Universal Gravity Formula. REQUIRES target has SpacePhysics component.
    /// </summary>
    /// <param name="target"></param>
    Vector2 GetForce(SpacePhysics target)
    {
        if (!emitsGravity || target == this) return Vector2.zero;

        // get all the values for the formula
        float r = (target.transform.position - transform.position).magnitude;
        Vector2 rHat = (transform.position - target.transform.position).normalized;
        float m1 = this.mass;
        float m2 = target.mass;
        if (m1 == 0) Debug.LogWarning(gameObject + " physics mass is 0.");
        if (m2 == 0) Debug.LogWarning(target.gameObject + " physics mass is 0.");
        // use the formula
        float vecMag = UNIVERSAL_GRAVITY_CONSTANT * m1 * m2 / Mathf.Pow(r, 2);
        Debug.Log(rHat.ToString() + " vmg " + vecMag);
        return rHat * vecMag;

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) DoPhysics();
    }
}
