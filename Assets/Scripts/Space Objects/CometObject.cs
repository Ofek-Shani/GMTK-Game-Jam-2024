using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometObject : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.SetComet(this);
        gm.AddSpacePhysicsObject(GetComponent<SpacePhysics>());
    }

    public int resources { get; private set; } = 0;
    public void Collect(ResourceTypeEnum.ResourceType toCollect)
    {
        resources++;
    }

    public IEnumerator Launch()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<TrailRenderer>().autodestruct = true;
    }

    /// <summary>
    /// Locks comet position and makes it invisible -- once the trail runs out the autodestruct feature 
    /// in the trail component deletes the object.
    /// </summary>
    public void LockAndDelete()
    {
        gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
        gm.currentComet = null;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
