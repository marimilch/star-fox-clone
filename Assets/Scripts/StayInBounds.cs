using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInBounds : MonoBehaviour
{
    [SerializeField] private GameObject offsetObject;
    private Vector3 offset;

    [SerializeField] private float horizontalBound = 15f;
    [SerializeField] private float verticalBound = 10f;

    // Start is called before the first frame update
    void Start()
    {
        if (!offsetObject)
        {
            offsetObject = GameObject.Find("Player");
        }
        offset = offsetObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var initialPos = transform.position;

        if (transform.position.x > horizontalBound + offset.x)
        {
            transform.position = new Vector3(
                horizontalBound + offset.x,
                initialPos.y,
                initialPos.z
            );
            initialPos = transform.position;
        } else if (transform.position.x < -horizontalBound + offset.x)
        {
            transform.position = new Vector3(
                -horizontalBound + offset.x,
                initialPos.y,
                initialPos.z
            );
            initialPos = transform.position;
        }

        if (transform.position.y > verticalBound + offset.y)
        {
            transform.position = new Vector3(
                initialPos.x,
                verticalBound + offset.y,
                initialPos.z
            );
        } else if (transform.position.y < -verticalBound + offset.y)
        {
            transform.position = new Vector3(
                initialPos.x,
                -verticalBound + offset.y,
                initialPos.z
            );
        }
    }
}
