using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float minRotSpeed = -10f, maxRotSpeed = 10f;
    float rotSpeed;

    private void Awake()
    {
        rotSpeed = Random.Range(minRotSpeed, maxRotSpeed);
    }

    private void Update()
    {
        transform.eulerAngles = transform.eulerAngles += new Vector3(0, 0, 1) * rotSpeed * Time.deltaTime;
    }
}
