using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastDamage : MonoBehaviour, IDamager
{
    [SerializeField] float damageAmount = 10f;
    Damage damage;
    GameObject owner;

    private void Awake()
    {
        damage = new Damage(damageAmount);
    }

    public Damage GetDamage()
    {
        return damage;
    }

    public void DamageInflicted()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public GameObject GetOwner()
    {
        return owner;
    }

    public void SetOwner(GameObject o)
    {
        owner = o;
    }
}
