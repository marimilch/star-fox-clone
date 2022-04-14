using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInBoundsSelf : MonoBehaviour
{
    [SerializeField] float backBound = 100f;
    [SerializeField] float frontBound = -10f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z < frontBound || transform.position.z > backBound)
        {
            gameObject.SetActive(false);
        }
    }
}
