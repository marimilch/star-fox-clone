using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour
{
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioClip incomingEnemy;

    [SerializeField] GameObject boss;

    AudioSource soundPlayer;

    bool wasActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wasActivated)
        {
            return;
        }
        wasActivated = true;
        soundPlayer.PlayOneShot(incomingEnemy);
        MusicController.SharedComponent.SetMusic(bossMusic);
        if (boss)
        {
            boss.SetActive(true);
        }
    }
}
