using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public static BlinkEffect SharedComponent;

    float passedTime;
    [SerializeField] float blinkInterval = .5f;
    [SerializeField] bool shareComponent;

    int numberOfChildren;
    List<BlinkObserver> blinkers;
    bool blinking = false;

    private string currentColor = "initial";

    private void Awake()
    {
        if (shareComponent)
        {
            SharedComponent = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        numberOfChildren = 0;
        var len = transform.childCount;
        blinkers = new List<BlinkObserver>();

        for (int i = 0; i < len; ++i)
        {
            var maybeBlinker = transform.GetChild(i).GetComponent<BlinkObserver>();
            if (maybeBlinker)
            {
                blinkers.Add(maybeBlinker);
                ++numberOfChildren;
            }
        }
    }

    private void FixedUpdate()
    {
        if (blinking)
        {
            passedTime += Time.deltaTime;
            if (passedTime > blinkInterval)
            {
                passedTime -= blinkInterval;
                Blink();
            }
        }  
    }

    public void StartBlink(float duration)
    {
        passedTime = 0f;
        blinking = true;
        Blink(); //directly become white
        StartCoroutine(StopAfter(duration));
    }

    IEnumerator StopAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopBlink();
    }

    void Blink()
    {
        //yield return new WaitForSeconds(shakeInterval);
        if (currentColor == "initial")
        {
            SetCurrentColorAndMaterials("white");
        }
        else
        {
            SetCurrentColorAndMaterials("initial");
        }
    }

    private void SetCurrentColorAndMaterials(string color)
    {
        SetAllMaterials(color);
        currentColor = color;
    }

    public void StopBlink()
    {
        SetCurrentColorAndMaterials("initial");
        blinking = false;
    }

    private void SetAllMaterials(string color)
    {
        for (int i = 0; i < numberOfChildren; ++i)
        {
            blinkers[i].SetMaterial(color);
        }
    }
}
