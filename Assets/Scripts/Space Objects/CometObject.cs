using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometObject : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.AddSpacePhysicsObject(GetComponent<SpacePhysics>());
    }

    public int[] resources { get; private set; } = new int[4];
    public void Collect(ResourceTypeEnum.ResourceType toCollect)
    {
        resources[(int)toCollect]++;
    }

    public void LockAndDelete()
    {
        gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
