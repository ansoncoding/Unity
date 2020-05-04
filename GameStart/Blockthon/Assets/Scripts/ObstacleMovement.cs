using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleMovement : MonoBehaviour
{
    float[] LEVELFORCES = { 3000f, 4000f, 6000f, 8000f };   
    
    public Rigidbody rb;
    private float forwardForce = 3000f;

    public void SetSpeed(int level)
    {
        if (level > 4)
        {
            throw new Exception("Invalid level");
        }
        forwardForce = LEVELFORCES[level-1];
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
