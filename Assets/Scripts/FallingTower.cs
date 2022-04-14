using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTower : MonoBehaviour
{
    [SerializeField] float distanceFall = 10f;
    [SerializeField] float fallSpeed = 10f;

    GameObject player;
    bool fallTriggered = false;
    float fallDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (
            transform.position.z - player.transform.position.z < distanceFall &&
            !fallTriggered
        )
        {
            Debug.Log("Lets fall");
            fallTriggered = true;
            if (transform.position.x > player.transform.position.x)
            {
                fallDirection = 1f;
            } else
            {
                fallDirection = -1f;
            }

        }

        if (fallTriggered)
        {
            transform.eulerAngles +=
                fallSpeed * fallDirection * Vector3.forward * Time.deltaTime;
        }
    }
}
