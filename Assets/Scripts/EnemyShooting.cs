using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    [SerializeField] bool directionDependant = false;
    [SerializeField] Vector3 blastSpawnOffset;
    [SerializeField] float tDistance = .5f;
    [SerializeField] float delay = 2f;
    [SerializeField] float duration = 2f;
    [SerializeField] string shootInstanceName = "Blast Enemy";

    bool shooting = false;
    
    float tDelta = 0f;

    //private void OnEnable()
    //{
    //    //ReadyToShoot();
    //}

    public void ReadyToShoot()
    {
        StartCoroutine("ShootLifeTime");
    }

    IEnumerator ShootLifeTime()
    {
        yield return new WaitForSeconds(delay);
        //Start shooting before first delta time has passed

        StartShooting();
        yield return new WaitForSeconds(duration);

        StopShooting();
    }

    public void StartShooting()
    {
        shooting = true;
    }

    public void StopShooting()
    {
        shooting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shooting)
        {
            tDelta += Time.deltaTime;

            if (tDelta >= tDistance)
            {
                Debug.Log("SHOOT");
                Shoot();
                tDelta %= tDistance;
            }
        }   
    }

    void Shoot()
    {
        var beam = SpawnManager.SharedInstance.GetInstance(shootInstanceName);

        //Debug.Log(beam);

        if (beam)
        {
            beam.transform.position =
            transform.position + transform.TransformDirection(blastSpawnOffset);
            if (directionDependant)
            {
                beam.transform.rotation = transform.rotation;
            } else
            {
                //shoot directly at player independently of own direction
                beam.transform.rotation = Quaternion.FromToRotation(
                    Vector3.forward,
                    (
                        PlayerController.SharedComponent.GetPosition() -
                        transform.position
                    )
                );
            }
        }
        else
        {
            Debug.LogWarning("No enemy beams left.");
        }
    }
}
