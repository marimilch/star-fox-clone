using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFire : MonoBehaviour
{
    [SerializeField] private float wingDistanceFromOrigin = 1.5f;

    void OnFire()
    {
        if (PlayerController.SharedComponent.ControlsEnabled())
        {
            var beam = SpawnManager.SharedInstance.GetInstance("Blast");
            if (beam)
            {
                beam.GetComponent<Shoot>().SetModel(Shoot.blastMode);
            }
        }
    }
}
