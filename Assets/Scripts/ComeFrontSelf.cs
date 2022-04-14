using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeFrontSelf : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        MakeMove();
    }

    public void MakeMove()
    {
        var speed = GameManager.SharedComponent.speedTimed;
        transform.position +=
            Vector3.back * speed;
    }
}
