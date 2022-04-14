using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotEmitter : MonoBehaviour
{
    [SerializeField] private GameObject dot;

    [SerializeField] private int xLength = 10;
    [SerializeField] private int zLength = 20;
    [SerializeField] private float xDistance = 1f;
    [SerializeField] private float zDistance = 1f;

    private Vector3 parentInitialPosition;
    private GameObject[] pool;

    // Start is called before the first frame update
    void Start()
    {
        parentInitialPosition = transform.position;
        pool = new GameObject[xLength * zLength];

        var cameraPos = GameObject.Find("Main Camera").transform.position;

        var startPos = new Vector3(
            -xLength * xDistance / 2, // x from mid
            0f,
            cameraPos.z // z from camera z
        );

        for (int i = 0; i < zLength; i++)
        {
            for (int j = 0; j < xLength; j++)
            {
                var pos =
                    startPos + Vector3.right * xDistance * j //x position
                             + Vector3.forward * zDistance * i //z position
                ;

                var newDot = Instantiate(dot, pos, dot.transform.rotation);

                newDot.transform.SetParent(transform);

                pool[i * xLength + j] = newDot;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentSpeed = GameManager.SharedComponent.speed;

        transform.position += Vector3.back * currentSpeed * Time.deltaTime;

        if (Mathf.Abs(transform.position.z) > zDistance){
            transform.position -= Vector3.back * zDistance;
        }
    }
}
