using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IGameObserver
{
    // Start is called before the first frame update
    public Rigidbody rb;
    
    float sidewaysForce = 100f;
    float jumpForce = 800f;
    float maxHeight = 2f;

    bool moveRight = false;
    bool moveLeft = false;
    bool jump = false;
    bool fired = false;
    bool canJump = false;
    bool canFire = false;

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
        if (level == 2)
        {
            CanJump();
        }
        else if (level == 3)
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

        if (rb.position.y < -1)
        {
            FindObjectOfType<GameManager>().EndGame();
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
        GameManager.Instance.AddLevelObserver(this);
    }
}
