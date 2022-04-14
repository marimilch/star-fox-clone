using System;
using UnityEngine;
public interface IDamager
{
    Damage GetDamage();
    void DamageInflicted();
}
