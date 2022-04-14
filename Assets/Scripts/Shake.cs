using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public static Shake SharedComponent;

    [SerializeField] private float shakeInterval = .5f;

    private float strength;
    private Vector3 initialPos;
    private GameObject redTint;
    private GameObject blueTint;

    private float passedTime;
    private bool manualMode = false;

    private void Awake()
    {
        SharedComponent = this;

        blueTint = transform.Find("Blue Tint").gameObject;
        redTint = transform.Find("Red Tint").gameObject;

        blueTint.SetActive(true);
        redTint.SetActive(false);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!manualMode)
        {
            passedTime += Time.deltaTime;
            if (passedTime > shakeInterval)
            {
                passedTime -= shakeInterval;
                OneShake();
            }
        }    
    }

    public void StartShake(float duration, float strength)
    {
        passedTime = 0f;
        gameObject.SetActive(true);
        this.strength = strength;
        StartCoroutine(StopAfter(duration));
    }

    Vector3 RandomXYPos(float range)
    {
        return new Vector3(
            Random.Range(-range, range),
            Random.Range(-range, range),
            0
        ).normalized;
    }

    void OneShake()
    {
        //yield return new WaitForSeconds(shakeInterval);
        CameraController.SharedComponent.customOffset = RandomXYPos(strength);
        SwapTints();
    }

    IEnumerator StopAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (!manualMode)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (CameraController.SharedComponent)
        {
            CameraController.SharedComponent.customOffset = Vector3.zero;
        }
    }

    void SwapTints()
    {
        blueTint.SetActive(!blueTint.activeSelf);
        redTint.SetActive(!redTint.activeSelf);
    }

    public void RedTintify()
    {
        manualMode = true;
        gameObject.SetActive(true);
        redTint.SetActive(true);
        blueTint.SetActive(false);
    }
}
