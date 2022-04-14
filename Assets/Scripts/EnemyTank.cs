using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour, IReactToDeath
{
    [SerializeField] float driveSpeed = 10f;
    [SerializeField] float turnSpeed = 1f;

    GameObject player;
    bool alive = true;

    Vector3 directionToPlayer;
    Vector3 goalDirection;
    Vector3 currentDirection;

    Quaternion goalRotation;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Debug.Log("Started Lifetime");
        directionToPlayer =
            (player.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!alive)
        {
            return;
        }

        goalDirection = transform.TransformDirection(Vector3.left);

        //var currentGoalAngle = Mathf.Acos(
        //    Vector3.Dot(currentDirection, goalDirection)
        //);
        //var rotationAxis = Vector3.Cross(
        //    currentDirection,
        //    goalDirection
        //);
        //var rotationAngle =
        //    Mathf.Min(maxRotationSpeed * Time.deltaTime, currentGoalAngle);

        //var rotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);

        currentDirection =
            Vector3.RotateTowards(
                currentDirection,
                goalDirection,
                turnSpeed * Time.deltaTime,
                1f
            ).normalized
        ;

        var rotateY = Vector3.SignedAngle(
            Vector3.forward,
            FlattenOn(currentDirection, "y"),
            Vector3.up
        );

        transform.rotation = Quaternion.Euler( 0 , rotateY, 0 );

        var rotateX = Vector3.SignedAngle(
            transform.TransformDirection(Vector3.forward),
            currentDirection,
            transform.TransformDirection(Vector3.right)
        );

        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);

        //var angleDelta =
        transform.position +=
            currentDirection * driveSpeed * Time.deltaTime;
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

    public void OnDie()
    {
        alive = false;
    }
}
