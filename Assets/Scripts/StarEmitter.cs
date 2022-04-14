using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEmitter : MonoBehaviour
{
    [SerializeField] private GameObject[] stars;

    [SerializeField] private float startDistance = 10f;
    [SerializeField] private float xReach = 10f;
    [SerializeField] private float yReach = 10f;
    [SerializeField] private float deltaTime = .5f;
    [SerializeField] private string objectNamePrefix = "Star ";
    [SerializeField] private int maxObjectName = 4;

    [SerializeField] private bool spawningFollowsPlayer = true;

    GameObject player; 

    private Vector3 parentInitialPosition;
    private GameObject[] pool;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnStar", 0f, deltaTime);
        if (spawningFollowsPlayer)
        {
            player = GameObject.Find("Player");
        }
    }

    void SpawnStar()
    {
        var star =
            SpawnManager
            .SharedInstance.GetInstance(
                objectNamePrefix + Random.Range(1, maxObjectName+1)
            )
        ;

        if (star != null)
        {
            var offset =
                spawningFollowsPlayer ?
                    new Vector3(
                        player.transform.position.x,
                        player.transform.position.y,
                        0f
                     ) :
                    Vector3.zero
            ;
            star.transform.position = RandStartPos() + offset;
        }
    }

    Vector3 RandStartPos()
    {
        var x = xReach * Random.Range(-1f, 1f);
        var y = yReach * Random.Range(-1f, 1f);

        var xAbs = Mathf.Abs(x);
        var yAbs = Mathf.Abs(y);

        return new Vector3(
            x,
            y,
            startDistance * Mathf.Cos(
                (xAbs + yAbs)/(xReach+yReach)*Mathf.PI/2
            )
        );
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    var currentSpeed = GameManager.SharedComponent.speed;

    //    transform.position += Vector3.back * currentSpeed * Time.deltaTime;

    //    if (Mathf.Abs(transform.position.z) > aliveDistance){
    //        transform.position -= Vector3.back * zDistance;
    //    }
    //}
}
