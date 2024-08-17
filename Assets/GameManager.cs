using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    List<SpaceObjectLauncher> launchers = new();

    const float SPACE_DUST_SPEED_MULTIPLIER = 0.1f;
    ParticleSystem particleSystem;

    enum Ammo { Comet, Asteroid };
    public int startingAsteroids, startingComets;
    int[] ammoRemaining;
    [SerializeField] List<GameObject> ammoPrefabs;
    Ammo currentAmmoType = 0;
    public bool canLaunchersFire { private set; get; } = true;


    private void Awake()
    {
        ammoRemaining = new int[] { startingComets, startingAsteroids };
        particleSystem = GetComponent<ParticleSystem>();
    }   

    private void Start()
    {
        var launcherObjects = GameObject.FindGameObjectsWithTag("Launcher");
        foreach (var launcherObject in launcherObjects) launchers.Add(launcherObject.GetComponent<SpaceObjectLauncher>());
        SwitchAmmo(Ammo.Comet);

    }

    private void FixedUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        particleSystem.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++) particles[i].velocity = GetNetParticleVelocity(particles[i].position);
        particleSystem.SetParticles(particles, particleSystem.particleCount);
    }

    Vector2 GetNetParticleVelocity(Vector2 particlePos)
    {
        Vector2 toReturn = Vector2.zero;
        foreach (SpacePhysics sp in spacePhysicsInScene) toReturn += sp.GetParticleVelocity(particlePos);

        return toReturn * SPACE_DUST_SPEED_MULTIPLIER;
    }

    private void Update()
    {
        // Number Key Input handling
        int numberInput = GetPressedNumber();
        if(numberInput != -1)
        {
            Debug.Log(numberInput + " Pressed.");
            if (ammoRemaining[numberInput] > 0) SwitchAmmo((Ammo)numberInput);
        }

        // Restart Key Input handling
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloading Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // PHYSICS CONTROL
    public List<SpacePhysics> spacePhysicsInScene { get; private set; } = new();
    public void AddSpacePhysicsObject(SpacePhysics toAdd)
    {
        spacePhysicsInScene.Add(toAdd);
    }
    public void RemoveSpacePhysicsObject(SpacePhysics toRemove)
    {
        spacePhysicsInScene.Remove(toRemove);
    }

    // AMMO CONTROL

    int GetPressedNumber()
    {
        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))
                return number;
        }
        return -1;
    }

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
    }



}
