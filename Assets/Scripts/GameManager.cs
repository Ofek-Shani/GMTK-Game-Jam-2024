using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    List<SpaceObjectLauncher> launchers = new();

    const float SPACE_DUST_SPEED_MULTIPLIER = 0.5f;
    ParticleSystem particleSystem;

    HotbarController hotbar;
    TMP_Text resourceText;

    bool levelComplete = false;

    enum Ammo { Comet, Asteroid, Piercer, Pusher, Blaster};
    public int resourcesNeeded = 1;
    public int startingAsteroids, startingComets, startingPiercers, startingPushers, startingBlasters;
    public int[] ammoRemaining { get; private set; }
    public int[] maxAmmo { get; private set; }
    [SerializeField] List<GameObject> ammoPrefabs;
    Ammo currentAmmoType = 0;
    public bool canLaunchersFire { private set; get; } = true;

    public CometObject currentComet;
    public void SetComet(CometObject toSet)
    {
        if (currentComet) currentComet.LockAndDelete();
        currentComet = toSet;
    }

    public string nextLevel;

    private void Awake()
    {
        ammoRemaining = new int[] { startingComets, startingAsteroids , startingPiercers, startingPushers, startingBlasters};
        maxAmmo = new int[] { startingComets, startingAsteroids, startingPiercers, startingPushers, startingBlasters };
        particleSystem = GetComponent<ParticleSystem>();
        hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<HotbarController>();
        resourceText = GameObject.FindGameObjectWithTag("ResourceTracker").GetComponentInChildren<TMP_Text>();
    }   

    private void Start()
    {
        var launcherObjects = GameObject.FindGameObjectsWithTag("Launcher");
        foreach (var launcherObject in launcherObjects) launchers.Add(launcherObject.GetComponent<SpaceObjectLauncher>());
        SwitchAmmo((int)Ammo.Comet);

    }

    private void FixedUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        particleSystem.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++) particles[i].velocity = GetNetParticleVelocity(particles[i].position);
        particleSystem.SetParticles(particles, particleSystem.particleCount);
        resourceText.text = (currentComet ? currentComet.resources + "/" : 0 + "/") + resourcesNeeded;
    }

    public IEnumerator Victory()
    {
        canLaunchersFire = false;
        levelComplete = true;
        yield return new WaitForSecondsRealtime(0.75f);
        GameObject.FindGameObjectWithTag("WinPanel").GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    /// <summary>
    /// Get net particle velocity due to all physics objects
    /// </summary>
    /// <param name="particlePos"></param>
    /// <returns></returns>
    Vector2 GetNetParticleVelocity(Vector2 particlePos)
    {
        Vector2 toReturn = Vector2.zero;
        foreach (SpacePhysics sp in spacePhysicsInScene) toReturn += sp.GetParticleVelocity(particlePos);

        return toReturn * SPACE_DUST_SPEED_MULTIPLIER;
    }

    private void Update()
    {

        if (!levelComplete)
        {
            // Number Key Input handling
            int numberInput = GetPressedNumber();
            if (numberInput != -1)
            {
                Debug.Log(numberInput + " Pressed.");
                if (ammoRemaining[numberInput] > 0) SwitchAmmo((Ammo)numberInput);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(nextLevel);
        // Restart Key Input handling
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloading Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // PHYSICS CONTROL
    public List<SpacePhysics> spacePhysicsInScene { get; private set; } = new();
    /// <summary>
    /// adds SpacePhysics to the GM list
    /// </summary>
    /// <param name="toAdd"></param>
    public void AddSpacePhysicsObject(SpacePhysics toAdd)
    {
        spacePhysicsInScene.Add(toAdd);
    }
    /// <summary>
    /// removes SpacePhysics from GM list
    /// </summary>
    /// <param name="toRemove"></param>
    public void RemoveSpacePhysicsObject(SpacePhysics toRemove)
    {
        spacePhysicsInScene.Remove(toRemove);
    }

    // AMMO CONTROL

    /// <summary>
    /// gets number of currently pressed number key, and -1 if nothing is pressed.
    /// </summary>
    /// <returns></returns>
    int GetPressedNumber()
    {
        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))
                return number;
        }
        return -1;
    }

    /// <summary>
    /// Switches the current ammo type to the given one
    /// </summary>
    /// <param name="newAmmo"></param>
    /// <returns></returns>
    bool SwitchAmmo(Ammo newAmmo)
    {
        if (ammoRemaining[(int)newAmmo] <= 0 || (int)newAmmo > ammoRemaining.Length) return false;
        foreach(SpaceObjectLauncher launcher in launchers)
        {
            launcher.objectToLaunch = ammoPrefabs[(int)newAmmo];
        }
        currentAmmoType = newAmmo;
        canLaunchersFire = true;
        Debug.Log("Ammo switched to " + newAmmo + "s (You have " + ammoRemaining[(int)newAmmo] + ").");
        hotbar.ChangeSelection((int)newAmmo);
        return true;
    }

    /// <summary>
    /// Called by launchers whenever they fire
    /// </summary>
    public void OnLauncherFired()
    {
        ammoRemaining[(int)currentAmmoType] -= 1;
        canLaunchersFire = ammoRemaining[(int)currentAmmoType] > 0;
        Debug.Log("Projectile " + currentAmmoType + " fired. " + ammoRemaining[(int)currentAmmoType] + "  rounds left.");
        hotbar.UpdateIcons();
    }



}
