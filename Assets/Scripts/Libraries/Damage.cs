using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public static readonly float defaultAmount = 10.0f;
    //public static readonly string defaultLocation = "body";

    public readonly float forceDuration = .2f;
    public readonly float amount;

    //public readonly string location;
    public readonly Vector2 forceDirection;
    public readonly bool keepInitialX = false;
    public readonly bool keepInitialY = false;

    public Damage()
    {
        amount = defaultAmount;
    }

    public Damage(float amount)
    {
        this.amount = amount;
    }

    public Damage(
        float amount,
        Vector2 forceDirection,
        bool keepInitialX,
        bool keepInitialY
    )
    {
        this.amount = amount;
        this.keepInitialX = keepInitialX;
        this.keepInitialY = keepInitialY;
        this.forceDirection = forceDirection;
    }

    public Damage(float amount, Vector2 forceDirection)
    {
        this.amount = amount;
        this.keepInitialX = false;
        this.keepInitialY = false;
        this.forceDirection = forceDirection;
    }

    public Damage(Vector2 forceDirection)
    {
        amount = defaultAmount;

        this.keepInitialX = false;
        this.keepInitialY = false;
        this.forceDirection = forceDirection;
    }
}
