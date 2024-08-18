using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceCloudObject : MonoBehaviour
{
    const float TIME_FROM_EXPLODE_TO_DESTROY = 1;

    public ResourceTypeEnum.ResourceType resource;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Pause();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.AddSpacePhysicsObject(GetComponent<SpacePhysics>());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<CometObject>() != null)
        {
            // update resource counter
            collider.gameObject.GetComponent<CometObject>().Collect(resource);
            
            gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
        }
    }

    public IEnumerator Explode()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(TIME_FROM_EXPLODE_TO_DESTROY);
        Destroy(gameObject);
    }
}
