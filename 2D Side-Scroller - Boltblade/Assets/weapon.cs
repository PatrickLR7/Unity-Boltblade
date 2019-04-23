using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject bullet;

    void Start()
    {
        InvokeRepeating("Shoot", 1.0f, 2.2f);
    }

    void Shoot()
    {
        Instantiate(bullet, FirePoint.position, FirePoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
