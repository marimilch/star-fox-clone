using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour, IReactToDeath
{
    [SerializeField] float maxRotationSpeed = 1f;
    [SerializeField] float flySpeedSpawn = 1f;
    [SerializeField] public float flySpeedFollow = 1f;
    [SerializeField] float waitUntilFollow = 2f;
    [SerializeField] float waitUntilFlyForward = 2f;

    [SerializeField] bool facePlayerDirectly = false;
    [SerializeField] public GameObject target;

    [SerializeField] bool neverFlyForward = false;

    float flySpeed = 0f;

    GameObject player;
    bool flyToScreen = false;
    bool followPlayer = false;
    bool alive = true;

    Vector3 directionToPlayer;
    Vector3 goalDirection;
    Vector3 currentDirection;

    Quaternion goalRotation;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void OnEnable()
    {
        //Debug.Log("Started Lifetime");
        MakeReady();
    }

    public void MakeReady()
    {
        ApplyTarget();
        alive = true;
        directionToPlayer =
            (player.transform.position - transform.position).normalized;

        if (facePlayerDirectly)
        {
            currentDirection = directionToPlayer;
        }

        StartCoroutine("Lifetime");
    }

    void ApplyTarget()
    {
        if (target == null)
        {
            player = GameObject.Find("Player");
        }
        else
        {
            player = target;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyTarget();
        if (!alive)
        {
            return;
        }

        if (!flyToScreen)
        {
            directionToPlayer =
                (player.transform.position - transform.position).normalized;
        } else
        {
            directionToPlayer = Vector3.back;
        }

        goalDirection = followPlayer ? directionToPlayer : Vector3.forward;

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
                maxRotationSpeed * Time.deltaTime,
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
            currentDirection * flySpeed * Time.deltaTime;
    }

    IEnumerator Lifetime()
    {
        followPlayer = false;
        flyToScreen = false;
        flySpeed = flySpeedSpawn;
        yield return new WaitForSeconds(waitUntilFollow);
        flySpeed = flySpeedFollow;
        followPlayer = true;

        if (!neverFlyForward)
        {
            yield return new WaitForSeconds(waitUntilFlyForward);
            flyToScreen = true;
        }
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
