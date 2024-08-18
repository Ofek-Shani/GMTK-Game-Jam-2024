using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObjectLauncher : MonoBehaviour
{
    // Spaceobj Simulation Constants
    const int SIM_NUM_STEPS = 100;
    const float SIM_STEP_SIZE = 1f;

    public float mouseCooldownTimer = 1f;
    float mouseCooldownCountdown = 0f;

    
    public float previewLifetime = 1;
    float previewTimer = 0;

    //component references
    LineRenderer lineRenderer;
    Camera cam;
    GameManager manager;

    public GameObject objectToLaunch;
    public GameObject trajectoryPreviewObject;
    GameObject toLaunchInstance;
    GameObject previewInstance;

    public float launchSpeed = 10;
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
        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && mouseCooldownCountdown <=0 && previewInstance)
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
            previewTimer = 0;
            launchVector = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

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
            if (previewTimer <= 0)
            {
                previewInstance = Instantiate(trajectoryPreviewObject, transform.position, Quaternion.identity);
                previewInstance.GetComponent<Rigidbody2D>().velocity = launchVector * launchSpeed;
                // not calling RemoveSpacePhysicsObject here might cause some bugs, oh well lol
                Destroy(previewInstance, previewLifetime);
                previewTimer = previewLifetime;
            }
            else
            {
                previewTimer -= Time.deltaTime;
            }

            launchVector = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

            // List<Vector2> positions = toLaunchInstance.GetComponent<SpacePhysics>().Simulate(launchVector.normalized * LAUNCH_SPEED, SIM_STEP_SIZE, SIM_NUM_STEPS);
            // Debug.Log(positions.Count);
            //lineRenderer.positionCount = positions.Count;
            //for (int i = 0; i < positions.Count; i++)
            //{
            //    lineRenderer.SetPosition(i, positions[i]);
            //}
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
        toLaunchInstance.GetComponent<Rigidbody2D>().velocity = launchVector.normalized * launchSpeed;
        toLaunchInstance.GetComponent<CircleCollider2D>().enabled = true;
        toLaunchInstance = null;
        aiming = false;
        lineRenderer.positionCount = 0;
        manager.OnLauncherFired();
    }
}
