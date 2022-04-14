using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamager
{
    [SerializeField] float damageAmount = 20f;
    [SerializeField] bool keepInitialX = true;
    [SerializeField] bool keepInitialY = true;
    [SerializeField] Vector3 forceVector;
    [SerializeField] float damageDelay = 1f;

    bool damageEnabled = true;

    Damage damage;

    private void Start()
    {
        damage = new Damage(
            damageAmount, forceVector, keepInitialX, keepInitialY
        );

        transform.tag = "Enemy";
    }

    void TemporarilyDisableDamage()
    {
        StartCoroutine("DisableDamageRoutine");
    }

    IEnumerator DisableDamageRoutine()
    {
        damageEnabled = false;
        yield return new WaitForSeconds(damageDelay);
        damageEnabled = true;
    }

    public Damage GetDamage()
    {
        if (damageEnabled)
        {
            //TemporarilyDisableDamage();
            return damage;
        }
        return null;
    }

    public void DamageInflicted()
    {

    }
}
