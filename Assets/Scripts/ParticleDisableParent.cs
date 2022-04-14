using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisableParent : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
