using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody rb;
    private float forwardForce = 3000f;
    private Vector3 scale;

    public void SetScale(Vector3 newScale)
    {
        scale = newScale;
    }

    public void SetSpeed(float newForwardForce)
    {
        forwardForce = newForwardForce;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.AddForce(0, 0, -forwardForce * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < GameManager.PLAYER_POSNZ)
        {
            GameManager.Instance.IncPoints();
            Destroy(gameObject);
        }
    }
}
