using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Functional;

public class FlyToLinear : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    [SerializeField] float margin = .1f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float flySpeed = 20f;
    [SerializeField] float maxZRotation = 45f;


    [SerializeField] bool disableWhenTargetReached = false;
    [SerializeField] bool steerOnly = false;

    Vector3 directionToTarget;

    Vector3 currentDirection;
    Vector3 localRight;
    float timedRotationSpeed;
    float timedFlySpeed;
    float lastZRotation;
    bool mutex = false;
    float localForwardRotation;

    //[SerializeField] GameObject debugReference;
    //[SerializeField] GameObject debugReference2;

    [SerializeField] public Action<Transform> onTargetReached;

    // Start is called before the first frame update
    //void Start()
    //{
    //    currentDirection = transform.TransformDirection(Vector3.forward);
    //}

    float Rotate(float angle, Vector3 axis)
    {
        var rotated = cleanAngle(angle);
        transform.Rotate(axis, rotated);

        return rotated;
    }

    float cleanAngle(float angle)
    {
        return 
            Mathf.Sign(angle) * Mathf.Min(
                Mathf.Abs(angle),
                timedRotationSpeed
            )
        ;
    }

    float Steer(float angle)
    {
        return Rotate(angle, Vector3.up);
    }

    float RotateDirectionAxis(float angle)
    {
        return Rotate(angle, currentDirection);
    }

    void SetZRotation(float zRotation)
    {
        //rotated is already timed
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            zRotation
        );
    }

    void SetTiltLeftRight(float rotated)
    {
        var angle = Mathf.Abs(rotated);
        var sign = Mathf.Sign(rotated);

        var dMaxAngle = Mathf.Abs(maxZRotation - localForwardRotation);

        var toRotate = sign * Mathf.Min(dMaxAngle, angle);

        localForwardRotation += toRotate;

        RotateDirectionAxis(toRotate);
    }

    float Tilt(float angle)
    {
        return Rotate(angle, localRight);
    }

    Vector3 FlattenOn(Vector3 v, string c)
    {
        var x = v.x;
        var y = v.y;
        var z = v.z;

        return new Vector3(
             c == "x" ? 0f : v.x,
             c == "y" ? 0f : v.y,
             c == "z" ? 0f : v.z
        );
    }

    void UpdateValues()
    {
        timedRotationSpeed = Time.deltaTime * rotationSpeed;
        timedFlySpeed = Time.deltaTime * flySpeed;
        directionToTarget = (target.position - transform.position).normalized;
        localRight = transform.TransformDirection(Vector3.right);
        currentDirection = transform.TransformDirection(Vector3.forward);
        lastZRotation = transform.localEulerAngles.z;
    }

    void CheckIfTargetReached()
    {
        if (
            Mathf.Abs(
                (transform.position - target.position - targetOffset).magnitude
            ) < margin
        )
        {
            //invoke only once, if reached
            if (!mutex)
            {
                onTargetReached?.Invoke(target);
                mutex = true;
            }
            if (disableWhenTargetReached)
            {
                enabled = false;
            }
        } else
        {
            mutex = false;
        }
    }

    float AngleDistanceOn(string coord)
    {
        var flatCurrentDirection = FlattenOn(currentDirection, coord);
        var flatDestinationDirection = FlattenOn(directionToTarget, coord);
        Vector3 rotationAxis;
        if (coord == "x")
        {
            rotationAxis = Vector3.right;
        }
        else if (coord == "y")
        {
            rotationAxis = Vector3.up;
        }
        else
        {
            rotationAxis = Vector3.forward;
        }

        return Vector3.SignedAngle(
            flatCurrentDirection,
            flatDestinationDirection,
            rotationAxis
        );
    }

    //float ShortestAngle(float angle)
    //{
    //    angle = angle % 360f;
    //    var sig = Mathf.Sign(angle);

    //    return Mathf.Abs(angle) >= 180f ? angle + -sig * 360f : angle;
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateValues();

        CheckIfTargetReached();

        // rotation
        if (!steerOnly)
        {
            Tilt(AngleDistanceOn("x"));
        }
        Steer(AngleDistanceOn("y"));
        SetZRotation(0f);


        //debugReference.transform.position =
        //    transform.position + directionToTarget*10f;

        //debugReference2.transform.position =
        //    transform.position + currentDirection * 10f;

        transform.position +=
            transform.TransformDirection(Vector3.forward) * timedFlySpeed;

        //SetTiltLeftRight(rotated);
    }
}
