using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController SharedComponent;
    private AudioSource musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        SharedComponent = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    public void SetMusic(AudioClip music, float delay = 0f)
    {
        musicPlayer.Stop();
        StartCoroutine(PlayMusicAfter(music, delay));
    }

    IEnumerator PlayMusicAfter(AudioClip music, float delay)
    {
        yield return new WaitForSeconds(delay);
        musicPlayer.PlayOneShot(music);
    }
}
