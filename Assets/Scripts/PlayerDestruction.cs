using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Functional;

public class PlayerDestruction : MonoBehaviour
{
    public static PlayerDestruction SharedComponent;

    [SerializeField] ParticleSystem explosionDeath;
    [SerializeField] float delay;

    GameObject player;

    private ParticleSystem explosionDeathInstance;

    private void Start()
    {
        SharedComponent = this;
        player = PlayerController.SharedComponent.fakeWing;

        explosionDeathInstance = Instantiate(
            explosionDeath,
            Vector3.zero,
            explosionDeath.transform.rotation
        );

        //explosionDeathInstance.transform.SetParent(transform);
    }

    public void PrepareDoom(Action after)
    {
        StartCoroutine(PrepareDoomRoutine(after));
    }

    IEnumerator PrepareDoomRoutine(Action after)
    {
        yield return new WaitForSeconds(delay);
        explosionDeathInstance.transform.position = player.transform.position;
        explosionDeathInstance.Play();
        explosionDeathInstance.GetComponent<AudioSource>().Play();

        player.SetActive(false);
        after();
        //GameManager.reloadAfter()
    }

}
