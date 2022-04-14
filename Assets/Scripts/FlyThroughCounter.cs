using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyThroughCounter : MonoBehaviour
{
    [SerializeField] string counterName;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            TunnelManager.SharedComponent.IncreaseCounter(counterName);
            gameObject.SetActive(false);
        }
    }
}
