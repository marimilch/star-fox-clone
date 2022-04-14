using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeFront : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MakeMove();
    }

    public void MakeMove()
    {
        transform.position +=
            Vector3.back * GameManager.SharedComponent.speedTimed;
    }
}
