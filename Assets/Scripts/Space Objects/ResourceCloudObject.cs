using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCloudObject : MonoBehaviour
{
    public ResourceTypeEnum.ResourceType resource;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            Destroy(gameObject);
        }
    }
}
