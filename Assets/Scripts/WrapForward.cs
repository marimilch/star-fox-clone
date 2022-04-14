using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapForward : MonoBehaviour
{
    private float tileLength;
    [SerializeField] private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        tileLength = 40f / 8f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.back / speed;
        if (Mathf.Abs(transform.position.z) > tileLength)
        {
            transform.position = Vector3.zero;
        }
    }
}
