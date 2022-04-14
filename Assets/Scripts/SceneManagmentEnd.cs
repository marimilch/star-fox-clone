using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagmentEnd : MonoBehaviour
{
    Transitions transitions;

    AudioSource audioSource;
    [SerializeField] AudioClip confirmSound;

    // Start is called before the first frame update
    void Start()
    {
        transitions = Transitions.SharedComponent;
        transitions.FadeIn();

        audioSource = GetComponent<AudioSource>();
    }


    public void EndGame()
    {
        audioSource.PlayOneShot(confirmSound);
        Transitions.SharedComponent.FadeOut(Quit);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
