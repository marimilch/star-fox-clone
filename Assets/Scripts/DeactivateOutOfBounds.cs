using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOutOfBounds : MonoBehaviour
{
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private bool followPlayer = false;

    GameObject player;

    private void Start()
    {
        player = PlayerController.SharedComponent.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var offset = followPlayer ? player.transform.position : Vector3.zero;
        if ( (transform.position - offset).magnitude > maxRange)
        {
            gameObject.SetActive(false);
        }
    }
}
