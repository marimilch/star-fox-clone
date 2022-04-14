using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCheck : MonoBehaviour, IDamager
{
    [SerializeField] float damageAmount = 20f;

    public void DamageInflicted()
    {
        
    }

    public Damage GetDamage()
    {
        return new Damage(damageAmount);
    }
}
