using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IGameObserver
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private float leftBoundary;
    private float rightBoundary;
    private float sidewaysForce = 100f;
    private float jumpForce = 800f;
    private float maxHeight = 2f;

    private bool moveRight = false;
    private bool moveLeft = false;
    private bool jump = false;
    private bool fired = false;
    private bool canJump = false;
    private bool canFire = false;

    public void CanFire()
    {
        canFire = true;
    }
    public void CanJump()
    {
        canJump = true;
    }

    public void Notify()
    {
        int level = GameManager.Instance.GetLevel();
        if (level == 3)
        {
            CanJump();
        }
        else if (level == 4)
        {
            CanFire();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveRight = true;   
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeft = true;
        }

        if (canJump && Input.GetKeyUp(KeyCode.Space))
        {
            jump = true;
        }

        if (canFire && Input.GetKeyUp(KeyCode.F))
        {
            fired = true;
        }

        if (rb.position.y < -1 || rb.position.x < leftBoundary || rb.position.x > rightBoundary)
        {
            GameManager.Instance.EndGame();
        }
    }

    // Use FixedUpdate for physics related updates
    void FixedUpdate()
    {
        // Multiply by Time.deltaTime because of different computer settings and frame rates

        if (jump)
        {
            if (rb.position.y < maxHeight)
            {
                rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
            }
            jump = false;
        }

        if (fired)
        {
            GameManager.Instance.CheckHit();
            fired = false;
        }

        if (moveLeft)
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            moveLeft = false;
        }

        if (moveRight)
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            moveRight = false;
        }    
    }

    // will be called if there is a box collider and rigid body
    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "Obstacle")
        {
            GameManager.Instance.EndGame();
        }
    }

    void Start()
    {
        leftBoundary = SpawnManager.PATH_LEFT - 0.5f;
        rightBoundary = SpawnManager.PATH_RIGHT + 0.5f;
        GameManager.Instance.AddLevelObserver(this);
    }
}
