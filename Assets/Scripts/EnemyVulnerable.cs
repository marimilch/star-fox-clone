using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Functional;

public class EnemyVulnerable : MonoBehaviour
{
    [SerializeField] string[] destructOn = new string[] { "Enemy", "Blast" };
    [SerializeField] bool comeFrontOnDie = false;
    [SerializeField] float health = 10f;
    [SerializeField] float receiveFactor = 1f;
    [SerializeField] bool reflect = false;
    [SerializeField] float blinkDuration = .5f;
    [SerializeField] AudioClip damageTakenSound;
    [SerializeField] bool addDefaultBlink = true;
    [SerializeField] bool addDefaultDie = true;

    public Action<Damage, string> onHit;
    public string messageOnHit;

    private BlinkEffect blinker;
    private AudioSource audioSource;

    private float initialHealth;

    //IReactToDeath reactToDeath;

    bool alive = true;

    // Start is called before the first frame update
    void Awake()
    {
        initialHealth = health;

        //reactToDeath = gameObject.GetComponent<IReactToDeath>();
        if (addDefaultBlink)
        {
            blinker = transform.GetChild(0).GetComponent<BlinkEffect>();
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Revive()
    {
        alive = true;
        health = initialHealth;
        blinker.StopBlink();
        gameObject.SetActive(true);
        //SendMessage("Revive");
    }

    // Prevent reviving on setting active again
    private void OnEnable()
    {
        if (!alive)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var g = other.gameObject;

        if (CompareWithAllTags(g))
        {
            //Debug.Log(g.tag);
            var damager = g.GetComponent<IDamager>();

            if (damager != null)
            {
                //Debug.Log(g.tag);
                //Debug.Log(gameObject);
                ReceiveDamage(damager.GetDamage());
                damager.DamageInflicted();
                //ShowDamageFX();
                DieIfNecessary();
                return;
            }

            Debug.LogWarning(
                g.tag + " was expected to have a IDamager Component."
            );
        }
    }

    void ReceiveDamage(Damage damage)
    {
        onHit?.Invoke(damage, messageOnHit);

        health -= receiveFactor * damage.amount;
        if (addDefaultBlink && blinker)
        {
            blinker.StartBlink(blinkDuration);
        }
        if (audioSource && damageTakenSound)
        {
            audioSource.PlayOneShot(damageTakenSound);
        }
    }

    void DieIfNecessary()
    {
        if (health <= 0f && addDefaultDie)
        {
            alive = false;
            var e = SpawnManager.SharedInstance.GetInstance("Small Explosion Particle");

            if (e != null)
            {
                var comeFront = e.GetComponent<ComeFront>();

                e.transform.position = transform.position;
                if (comeFront != null)
                {
                    if (comeFrontOnDie)
                    {
                        comeFront.enabled = true;
                    } else
                    {
                        comeFront.enabled = false;
                    }
                } 
                //e.GetComponent<ParticleSystem>().Play();
            }
                
            gameObject.SetActive(false);
        }     
    }

    bool CompareWithAllTags(GameObject g)
    {
        for (int i = 0; i < destructOn.Length; ++i)
        {
            if (g.CompareTag(destructOn[i]))
            {
                return true;
            }
        }
        return false;
    }
}
