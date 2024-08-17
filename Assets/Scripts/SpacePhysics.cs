using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

public class SpacePhysics : MonoBehaviour
{
    // GLOBAL PHYSICS CONSTANTS
    const float UNIVERSAL_GRAVITY_CONSTANT = 1f;

    // PER OBJECT CONFIGS

    bool isPaused = false;
    // max distance an object can be from this object to still recieve gravity force
    [Range(0.0f, 10.0f)]
    public float maxGravityDistance = 5f;
    public bool emitsGravity;
    public bool canMove;
    [Range(0.0f, 10.0f)]
    public float mass;
    public Vector2 initialVelocity;
    // Internal Components
    Rigidbody2D rb;
    // External components
    GameManager gm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
        rb.mass = mass;
        List<GameObject> otherPhysics = GameObject.FindGameObjectsWithTag("GravityEmitter").ToList<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame -- 50 times per second. 
    void FixedUpdate()
    {
        if (canMove && !isPaused)
        {
            //if (GetComponent<ResourceCloudObject>()) Debug.Log(" ASFD "  + otherBodies.ToString());
            rb.AddForce(GetNetForceVector(transform.position), ForceMode2D.Force);
        }

    }

    // PAUSING

    public void Pause() { isPaused = true; }
    public void Unpause() { isPaused = false; }

    // PHYSICS

    /// <summary>
    /// Gets the net force applied on this object by all other gravity emitters and uses Rigidbody to apply it.
    /// </summary>
    Vector2 GetNetForceVector(Vector2 position)
    {
        Vector2 netForce = Vector2.zero;
        foreach(SpacePhysics sp in gm.spacePhysicsInScene)
        {
            if(sp) netForce += sp.GetForce(this, position);

            // Debug.Log(netForce + " nf");
        }
        // Debug.Log(netForce);
        return netForce;
    }

    
    /// <summary>
    /// Gets the force exerted by this object onto the target gameObject using Universal Gravity Formula. REQUIRES target has SpacePhysics component.
    /// </summary>
    /// <param name="target"></param>
    Vector2 GetForce(SpacePhysics target, Vector2 targetPos)
    {

        //Debug.Log(targetPos.ToString() + " ASDFASD");
        if (!emitsGravity || target == this) return Vector2.zero;

        // get all the values for the formula
        float r = (transform.position - (Vector3)targetPos).magnitude;
        // if the object is too far away, do nothing.
        if(r > maxGravityDistance) return Vector2.zero;

        Vector2 rHat = (transform.position - (Vector3)targetPos).normalized;
        float m1 = this.mass;
        float m2 = target.mass;
        if (m1 == 0) Debug.LogWarning(gameObject + " physics mass is 0.");
        if (m2 == 0) Debug.LogWarning(target.gameObject + " physics mass is 0.");
        // use the formula
        float vecMag = UNIVERSAL_GRAVITY_CONSTANT * m1 * m2 / Mathf.Pow(r, 2);
        //Debug.Log(rHat.ToString() + " vmg " + vecMag);
        return rHat * vecMag;
    }

    /// <summary>
    /// Gets a particle velocity vector caused by this object.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetParticleVelocity(Vector2 pos)
    {
        Vector2 disp = pos - (Vector2)transform.position;
        return disp.normalized * (UNIVERSAL_GRAVITY_CONSTANT * this.mass / Mathf.Pow(disp.magnitude, 2));
    }

    public List<Vector2> Simulate(Vector2 initialVelocity, float stepSize, int numSteps)
    {
        List<Vector2> toReturn = new();
        Vector2 simPosition = transform.position;
        //Debug.Log(simPosition.ToString() + " simpos inint " + transform.position.ToString());
        Vector2 simVelocity = initialVelocity;
        toReturn.Add(transform.position);
        simPosition += simVelocity * stepSize;
        toReturn.Add(simPosition);

        for (int i = 0; i < numSteps; i++)
        {
            Vector2 force = GetNetForceVector(simPosition);
            simVelocity += force / mass * stepSize *Time.fixedDeltaTime /2;
            simPosition += simVelocity * stepSize * Time.fixedDeltaTime;
            toReturn.Add(simPosition);
        }
        return toReturn;
    }
}
