using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Functional;

public class GameManager : MonoBehaviour
{
    public static GameManager SharedComponent;
    public static bool invertYAxis = false;

    public float speed;
    public float speedTimed;

    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] float gameOverMusicDelay = .5f;
    [SerializeField] float fallFactor = .5f;
    [SerializeField] float fadeOutDelayDeath = 1f;

    AudioSource audioSource;

    private void Awake()
    {
        SharedComponent = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Transitions.SharedComponent.FadeIn();
        PlayerController.SharedComponent.invertYAxis = invertYAxis;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speedTimed = speed * Time.deltaTime;
    }

    void ReloadScene()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Start");
    }

    void FadeOutAndReload()
    {
        Transitions.SharedComponent.FadeOut(
            ReloadScene
        );
    }

    public void DelayedStart(Action action, float delay)
    {
        StartCoroutine(WaitThenStart(action, delay));
    }

    private IEnumerator WaitThenStart(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    void afterExplosion()
    {
        GameManager.SharedComponent.speed = 0;
        DelayedStart(
            FadeOutAndReload,
            fadeOutDelayDeath
        );
    }

    public void LifeLost()
    {
        MusicController.SharedComponent.SetMusic(
            gameOverMusic, gameOverMusicDelay
        );
        PlayerController.SharedComponent.DisableControls();
        PlayerController.SharedComponent.SimulateInputFor(Vector2.down*fallFactor, 20f);
        Shake.SharedComponent.RedTintify();
        CameraController.SharedComponent.Died();
        PlayerController.SharedComponent.floating.Died();
        BlinkEffect.SharedComponent.StartBlink(10f) ;
        PlayerDestruction.SharedComponent.PrepareDoom(afterExplosion);
        Shoot.blastMode = Shoot.NORMAL_BLAST;
    }

    public void OnPlayerExploded()
    {
        Transitions.SharedComponent.FadeOut();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
