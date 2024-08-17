using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObjectLauncher : MonoBehaviour
{
    // Spaceobj Simulation Constants
    const int SIM_NUM_STEPS = 500;
    const float SIM_STEP_SIZE = 1f;

    //component references
    LineRenderer lineRenderer;
    
    public GameObject objectToLaunch;
    GameObject toLaunchInstance;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    bool aiming = false;
    private void OnMouseDown()
    {
        aiming = true;
        toLaunchInstance = Instantiate(objectToLaunch, transform.position, new Quaternion());
        toLaunchInstance.GetComponent<SpacePhysics>().Pause();
        toLaunchInstance.transform.position = transform.position;

        // temporarily disable collider
        toLaunchInstance.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void OnMouseUp()
    {
        if(aiming)
        {
            aiming = false;
            Launch();
        }
    }

    private void OnMouseOver()
    {
        if (aiming)
        {
            List<Vector2> positions = toLaunchInstance.GetComponent<SpacePhysics>().Simulate(Vector2.left, SIM_STEP_SIZE, SIM_NUM_STEPS);
            // Debug.Log(positions.Count);
            lineRenderer.positionCount = positions.Count;
            for (int i = 0; i < positions.Count; i++)
            {
                lineRenderer.SetPosition(i, positions[i]);
            }
        }
    }

    private void OnMouseExit()
    {
        aiming = false;
        Destroy(toLaunchInstance);
        toLaunchInstance = null;

    }

    void Launch()
    {
        toLaunchInstance.GetComponent<SpacePhysics>().Unpause();
        toLaunchInstance.GetComponent<Rigidbody2D>().velocity = Vector2.left;
        toLaunchInstance.GetComponent<CircleCollider2D>().enabled = false;
        toLaunchInstance = null;
        aiming = false;
    }
}
