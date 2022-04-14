using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Functional;

public class BossHealth : MonoBehaviour
{
    public Dictionary<string, BossHealthData> data;

    [SerializeField] float blinkDuration = .5f;

    public static BossHealth SharedComponent;

    AudioSource audioSource;

    [SerializeField] AudioClip damageSound;

    BlinkEffect bossBodyBlinker;

    public Action onDie;

    // Start is called before the first frame update
    void Awake()
    {
        SharedComponent = this;
        bossBodyBlinker =
            transform.Find("Boss Body").GetComponent<BlinkEffect>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ApplyBossData(Dictionary<string, BossHealthData> data)
    {
        this.data = data;
        foreach (var thisDataKeyVal in data)
        {
            var thisData = thisDataKeyVal.Value;
            var thisDataPartsTransform = thisData.parts.transform;
            for (int i = 0; i < thisDataPartsTransform.childCount; ++i)
            {
                var c = thisDataPartsTransform.GetChild(i).GetComponent<EnemyVulnerable>();
                if (c)
                {
                    c.messageOnHit = thisDataKeyVal.Key;
                    c.onHit = Damage;
                }
            }
        }
    }

    public void Damage(Damage damage, string part) {
        //Debug.Log(part + ": " + damage.amount);
        if (data.ContainsKey(part) && data[part].active && data[part].health != 0f)
        {
            var thisData = data[part];
            thisData.blinker.StartBlink(blinkDuration);
            if (damageSound)
            {
                audioSource.PlayOneShot(damageSound);
            }
            thisData.health = Mathf.Max(thisData.health - damage.amount, 0f);

            if (thisData.health == 0f)
            {
                thisData.onLocationHealthZero();
            }
            //Debug.Log(part + " health: " + healths[part]);

            DieIfNecessary();
        }
    }

    void DieIfNecessary()
    {
        if (GetFullHealth() <= 0f)
        {
            onDie?.Invoke();
        }
    }

    float GetFullHealth()
    {
        var acc = 0f;

        foreach(var thisDataKV in data)
        {
            acc += thisDataKV.Value.health;
        }

        return acc;
    }
}
