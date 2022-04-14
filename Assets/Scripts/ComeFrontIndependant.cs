using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeFrontIndependant : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        MakeMove();
    }

    public void MakeMove()
    {
        transform.position +=
            Vector3.back * speed * Time.deltaTime;
    }
}
