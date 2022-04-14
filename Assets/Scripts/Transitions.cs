using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Functional;

public class Transitions : MonoBehaviour
{

    public static Transitions SharedComponent;
    [SerializeField] Image fade;

    [SerializeField] float fadeSpeed = 1.0f;
    float direction = 1.0f;

    Action afterAction;

    // Start is called before the first frame update
    void Awake()
    {
        SharedComponent = this;
        afterAction = Function.Pass;
    }

    private void Start()
    {
        //fade = Image.Find("Fade").GetComponent<Image>();
        fade.gameObject.SetActive(true);

        InstantSet(1f);
    }

    // Update is called once per frame
    void Update()
    {
        fade.color = new Color(
            fade.color.r,
            fade.color.g,
            fade.color.b,
            Mathf.Min(
                Mathf.Max(
                    fade.color.a + direction * Time.deltaTime * fadeSpeed,
                    0f
                ),
                1f
            )
        );

        if (fade.color.a == 1f || fade.color.a == 0f)
        {
            afterAction();
            gameObject.SetActive(false);
        }
    }

    void StartFade(float dir)
    {
        gameObject.SetActive(true);
        direction = dir;
    }

    public void FadeIn()
    {
        FadeIn(Function.Pass);
    }

    public void FadeOut()
    {
        FadeOut(Function.Pass);
    }

    public void FadeIn(Action a)
    {
        StartFade(-1f);
        afterAction = a;
    }

    public void FadeOut(Action a)
    {
        StartFade(1f);
        afterAction = a;
    }

    public void InstantSet(float toSet)
    {
        fade.color = new Color(
            fade.color.r,
            fade.color.g,
            fade.color.b,
            toSet
        );
    }
}
