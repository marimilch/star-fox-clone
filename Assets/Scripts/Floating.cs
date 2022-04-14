using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private const float round = Mathf.PI * 2;

    [SerializeField] private float duration = 1.0f;
    private float durationRotation;

    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float tilt = 10.0f;
    [SerializeField] private float timePassedMove = 0f;
    [SerializeField] private float timePassedRotation = 0f;
    [SerializeField] private float tiltMoveSyncFactor = 2f;
    [SerializeField] private float deathSpinSpeed = 2f;

    private float t = 0f;


    [SerializeField] private Vector3 offset;

    private bool died = false;


    // Start is called before the first frame update
    void Start()
    {
        durationRotation = duration * tiltMoveSyncFactor;
        offset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.deltaTime;
        if (died)
        {
            t += deltaTime;
        }

        //movement time
        timePassedMove += deltaTime;
        timePassedMove %= duration;

        //rotation time
        timePassedRotation += deltaTime;
        timePassedRotation %= durationRotation;

        var circlePartMove = timePassedMove / duration * round;
        var circlePartRotation = timePassedRotation / durationRotation * round;

        // up down float
        transform.localPosition =
            distance * Vector3.down * Mathf.Sin(circlePartMove) +
            offset
        ;

        // tilting
        transform.localEulerAngles = new Vector3(
            0,
            transform.localEulerAngles.y,
            tilt * Mathf.Sin(circlePartRotation)
                + (died ? deathSpinSpeed * t : 0f)
        );
    }

    public void Died()
    {
        died = true;
    }


}
