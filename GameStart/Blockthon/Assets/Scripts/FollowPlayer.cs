using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // we had to make a reference to the player because we want the camera to follow the player
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // when using "transform" we're referring to the camera or whatever asset is tied to this script (in this case it's the camera)
        transform.position = player.position + offset;
    }
}
