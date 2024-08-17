using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObjectLauncher : MonoBehaviour
{
    // Spaceobj Simulation Constants
    const int SIM_NUM_STEPS = 100;
    const float SIM_STEP_SIZE = 1f;

    //component references
    LineRenderer lineRenderer;
    Camera cam;
    GameManager manager;

    public GameObject objectToLaunch;
    GameObject toLaunchInstance;

    public float LAUNCH_SPEED = 2;
    Vector2 launchVector = Vector2.zero;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && mouseCooldownCountdown <=0)
        {
            previewTimer = 0;
            mouseCooldownCountdown = mouseCooldownTimer;
            manager.RemoveSpacePhysicsObject(previewInstance.GetComponent<SpacePhysics>());
            Destroy(previewInstance);
        }
        mouseCooldownCountdown -= Time.deltaTime;

    }


    bool aiming = false;
    private void OnMouseDown()
    {
        if (manager.canLaunchersFire)
        {
            aiming = true;
            toLaunchInstance = Instantiate(objectToLaunch, transform.position, new Quaternion());
            toLaunchInstance.GetComponent<SpacePhysics>().Pause();
            toLaunchInstance.transform.position = transform.position;

            // temporarily disable collider
            toLaunchInstance.GetComponent<CircleCollider2D>().enabled = false;
        }
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
            launchVector = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            List<Vector2> positions = toLaunchInstance.GetComponent<SpacePhysics>().Simulate(launchVector.normalized * LAUNCH_SPEED, SIM_STEP_SIZE, SIM_NUM_STEPS);
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
        if (aiming)
        {
            aiming = false;
            StartCoroutine(UnplaceProjectile());
            toLaunchInstance = null;
            lineRenderer.positionCount = 0;
        }

    }
    /// <summary>
    /// Runs
    /// </summary>
    public IEnumerator UnplaceProjectile()
    {
        toLaunchInstance.GetComponent<Animator>().Play("Disappear");
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(toLaunchInstance);
    }

    void Launch()
    {
        manager.RemoveSpacePhysicsObject(previewInstance.GetComponent<SpacePhysics>());
        Destroy(previewInstance);
        toLaunchInstance.GetComponent<SpacePhysics>().Unpause();
        toLaunchInstance.GetComponent<Rigidbody2D>().velocity = launchVector * LAUNCH_SPEED;
        toLaunchInstance.GetComponent<CircleCollider2D>().enabled = true;
        toLaunchInstance = null;
        aiming = false;
        lineRenderer.positionCount = 0;
        manager.OnLauncherFired();
    }
}
