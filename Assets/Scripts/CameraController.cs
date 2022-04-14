using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController SharedComponent;

    Vector3 offset;
    GameObject player;
    bool died = false;
    float t = 0f;
    float lastY;
    float PIHalf = Mathf.PI / 2f;

    [SerializeField] float followFactor;
    [SerializeField] float deathBackDistance = 20f;
    [SerializeField] float deathBackSpeed = 20f;
    [SerializeField] float deathSpinSpeed = 1f;
    [SerializeField] float deathResetFollowFactorSpeed = 1f;


    public Vector3 customOffset; // for shaking effect

    private void Awake()
    {
        SharedComponent = this;
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        //compensate for follow factor
        offset = new Vector3(
            (transform.position.x - player.transform.position.x) * (1f + followFactor),
            (transform.position.y - player.transform.position.y) * (1f + followFactor),
            transform.position.z - player.transform.position.z
        );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (died)
        {
            SetDeathMovement();
            t += Time.deltaTime;
        }

        transform.position = new Vector3(
            player.transform.position.x * followFactor,
            died ? lastY : player.transform.position.y * followFactor,
            player.transform.position.z
        ) + offset + customOffset;    
    }

    public void SetDeathMovement()
    {
        var deathMoveback = deathBackSpeed * t;
        if (deathMoveback > deathBackDistance)
        {
            deathMoveback = deathBackDistance;
        }

        followFactor = Mathf.Min(
            followFactor + deathResetFollowFactorSpeed * t,
            1
        );

        var spinT = deathSpinSpeed * t;

        var distanceToPlayer = offset.magnitude + deathMoveback;

        var rotationVec = distanceToPlayer * new Vector3(
            Mathf.Cos(spinT-PIHalf),
            0,
            Mathf.Sin(spinT-PIHalf)
        );

        var convertToDeg = 180f / Mathf.PI;
        transform.rotation =
            Quaternion.AngleAxis(-spinT* convertToDeg, Vector3.up);

        customOffset = rotationVec - offset;
    }

    public void Died()
    {
        died = true;
        lastY = transform.position.y;
    }
}
