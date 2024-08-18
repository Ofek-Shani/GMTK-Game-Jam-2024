using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceCloudObject : MonoBehaviour
{
    const float TIME_FROM_EXPLODE_TO_DESTROY = 1, TIME_FROM_COLLECT_TO_DESTROY = 1;

    public ResourceTypeEnum.ResourceType resource;
    GameManager gm;
    // Start is called before the first frame update

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<CometObject>() != null)
        {

            collider.gameObject.GetComponent<CometObject>().Collect(resource);
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        Debug.Log("Collecting Resource");
        GetComponent<Animator>().Play("Collect");
        gm.RemoveSpacePhysicsObject(GetComponent<SpacePhysics>());
        yield return new WaitForSecondsRealtime(TIME_FROM_COLLECT_TO_DESTROY);
        Destroy(gameObject);
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
