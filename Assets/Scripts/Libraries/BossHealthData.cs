using System;
using UnityEngine;
using Functional;

public class BossHealthData
{
    public Functional.Action onLocationHealthZero;
    //public Functional.Action<Damage, string> onDamageSubPart;
    public GameObject parts;
    public float health;
    public BlinkEffect blinker;
    public bool active = false;

    public BossHealthData(
        //string locationName,
        Functional.Action onLocationHealthZero,
        //Functional.Action<Damage, string> onDamageSubPart,
        GameObject parts,
        float health
    )
    {
        this.onLocationHealthZero = onLocationHealthZero;
        //this.onDamageSubPart = onDamageSubPart;
        this.parts = parts;
        this.health = health;
        blinker = parts.GetComponent<BlinkEffect>();
    }
}
