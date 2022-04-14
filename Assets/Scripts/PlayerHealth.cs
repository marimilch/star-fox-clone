using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth SharedComponent;
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float largeDamageSoundThreshold = .25f;
    [SerializeField] private float damageDelay = .25f;
    [SerializeField] private Damage damage;
    [SerializeField] private float invincibleTime;
    [SerializeField] private float noDamageTime;
    [SerializeField] private float maxShakeDurationOnDamage = .3f;
    [SerializeField] private float maxShakeStrengthOnDamage = .3f;
    [SerializeField] private float healthMeterWidth = 100f;

    [SerializeField] private AudioClip smallDamage;
    [SerializeField] private AudioClip largeDamage;
    [SerializeField] private AudioClip aboutToDie;
    [SerializeField] private float aboutToDieDelay = .5f;

    [SerializeField] private Image healthMeter;
    private bool dead;
    private bool damageReceived;
    private bool applyDamageNow;
    private bool invincible;
    private bool noDamage;
    private float initialHealth;
    private AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {
        SetHealthP(1f);
        SharedComponent = this;
        initialHealth = health;
        sound = GetComponent<AudioSource>();
    }

    void SetHealthP(float p)
    {
        if (healthMeter != null)
        {
            healthMeter.rectTransform.sizeDelta = new Vector2(
                healthMeterWidth * p, healthMeter.rectTransform.sizeDelta.y
            );
        }    
    }

    private void Update()
    {
        if (damageReceived)
        {
            damageReceived = false;
            applyDamageNow = true;
        }
    }

    //apply damage on next frame
    private void FixedUpdate()
    {
        if (applyDamageNow)
        {
            applyDamageNow = false;
            StartCoroutine(ApplyDamage(damage));
        }
    }

    // Damage is received with a slight delay
    public void ReceiveDamage(Damage damage)
    {
        damageReceived = true;
        this.damage = damage;
    }

    IEnumerator ApplyDamage(Damage damage)
    {
        yield return new WaitForSeconds(damageDelay);

        //force movement always 
        PlayerController.SharedComponent.ForceMovement(
            damage.forceDirection,
            damage.forceDuration,
            damage.keepInitialX,
            damage.keepInitialY
        );

        //update health meter
        SetHealthP(health/initialHealth);

        if (!invincible)
        {
            health -= noDamage ? 0 : damage.amount;

            StartCoroutine(InvincibleFor());
            StartCoroutine(NoDamageFor());

            //play sound
            if (!dead)
            {
                PlayDamageFX(damage.amount);
                Shake.SharedComponent.StartShake(
                    maxShakeDurationOnDamage * damage.amount / initialHealth,
                    maxShakeStrengthOnDamage * damage.amount / initialHealth
                );
                BlinkEffect.SharedComponent.StartBlink(
                    maxShakeDurationOnDamage * damage.amount / initialHealth
                );
                CheckIfLifeLost();

            }
        }   
    }

    void CheckIfLifeLost()
    {
        if (health <= 0f)
        {
            dead = true;
            StartCoroutine(PrepareDeath());
        }
    }

    IEnumerator PrepareDeath()
    {
        yield return new WaitForSeconds(aboutToDieDelay);
        sound.PlayOneShot(aboutToDie);
        GameManager.SharedComponent.LifeLost();
    }

    void PlayDamageFX(float amount)
    {
        if (amount > largeDamageSoundThreshold * initialHealth)
        {
            sound.PlayOneShot(largeDamage);
            return;
        }

        sound.PlayOneShot(smallDamage);
    }

    IEnumerator InvincibleFor()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }

    IEnumerator NoDamageFor()
    {
        noDamage = true;
        yield return new WaitForSeconds(noDamageTime);
        noDamage = false;
    }
}
