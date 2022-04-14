using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagment : MonoBehaviour
{
    Transitions transitions;
    [SerializeField] TextMeshProUGUI flipYAxisText;
    [SerializeField] TextMeshProUGUI loadingText;

    AudioSource audioSource;
    [SerializeField] AudioClip confirmSound;

    // Start is called before the first frame update
    void Start()
    {
        transitions = Transitions.SharedComponent;
        transitions.FadeIn();

        if (!GameManager.invertYAxis)
        {
            flipYAxisText.text = "no";
        }

        audioSource = GetComponent<AudioSource>();

        ////Set Preview to upper left corner
        //var screenRes = Screen.currentResolution;
        //screenX = (float) screenRes.width;
        //screenY = (float) screenRes.height;

        //screenPreview.rect = new Rect(
        //    screenX * previewXPercentage,
        //    screenY * previewYPercentage,
        //    screenX * previewWidthPercentage,
        //    screenY * previewHeightPercentage
        //);
    }

    // Update is called once per frame
    void Update()
    {
        //for testing
        //screenPreview.rect = new Rect(
        //    screenX * previewXPercentage,
        //    screenY * previewYPercentage,
        //    screenX * previewWidthPercentage,
        //    screenY * previewHeightPercentage
        //);
    }

    public void StartGame()
    {
        audioSource.PlayOneShot(confirmSound);
        Transitions.SharedComponent.FadeOut(LoadCorneria);
    }

    public void EndGame()
    {
        Transitions.SharedComponent.FadeOut(Quit);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void FlipYAxis()
    {
        var currentState = GameManager.invertYAxis;

        GameManager.invertYAxis = !currentState;
        PlayerController.SharedComponent.invertYAxis = !currentState;

        flipYAxisText.text = !currentState ? "yes" : "no";

    }

    private void LoadCorneria()
    {
        MusicController.SharedComponent.StopMusic();
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadSceneAsync("Corneria");
    }
}
