using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarObject : MonoBehaviour
{
    GameManager gm;
    Transform corona;

    const float CORONA_ROT_SPEED = 5;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.AddSpacePhysicsObject(GetComponent<SpacePhysics>());
        corona = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        corona.eulerAngles = corona.eulerAngles + CORONA_ROT_SPEED * Time.deltaTime * new Vector3(0, 0, 1);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        gm.RemoveSpacePhysicsObject(collider.GetComponent<SpacePhysics>());
        Destroy(collider.gameObject);
    }
}
