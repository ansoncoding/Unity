using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody rb;
    private float forwardForce = 0f;
    private bool pointsAwarded = false;

    public void SetScale(Vector3 newScale)
    {
        gameObject.transform.localScale = newScale;
    }

    public void SetSpeed(float newForwardForce)
    {
        //Debug.Log("New speed " + newForwardForce);
        forwardForce = newForwardForce;
    }

    public void SetPositionOffset(Vector3 offset)
    {
        //Debug.Log("Old position " + gameObject.transform.position);
        gameObject.transform.position += offset;
        //Debug.Log("New position " + gameObject.transform.position);
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
        if (!pointsAwarded && transform.position.z < GameManager.PLAYER_POSNZ)
        {
            pointsAwarded = true;
            GameManager.Instance.IncPoints();
        }
        
        if (transform.position.z < (GameManager.PLAYER_POSNZ - 5)) { 
                Destroy(gameObject);
        }
    }
}
