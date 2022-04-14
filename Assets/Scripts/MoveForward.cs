using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] Vector3 dir = Vector3.forward;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position +=
            transform.TransformDirection(dir) *
            Time.deltaTime *
            speed
        ;
    }
}
