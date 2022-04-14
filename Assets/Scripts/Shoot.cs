using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public static int blastMode = NORMAL_BLAST;

    [SerializeField] private float speed = 1.0f;
    public Vector3 offset;

    public static readonly int NORMAL_BLAST = 0;
    public static readonly int WING_BLAST = 1;
    public static readonly int SUPER_BLAST = 2;

    [SerializeField] private GameObject[] blasts;

    private Vector3 forward;

    GameObject player;

    private void Awake()
    {
        blasts = new GameObject[3];

        blasts[NORMAL_BLAST] = transform.Find("Normal Blast").gameObject;
        blasts[WING_BLAST] = transform.Find("Wing Blast").gameObject;
        blasts[SUPER_BLAST] = transform.Find("Super Blast").gameObject;

        SetModel(blastMode);
    }

    public void SetModel(int blastMode)
    {
        DeactivateAll();
        //Debug.Log(blasts.Length);
        blasts[blastMode].SetActive(true);
    }

    private void DeactivateAll()
    {
        for (int i = 0; i < blasts.Length; i++)
        {
            blasts[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player");
        transform.position = player.transform.position + offset;
        transform.rotation = player.transform.rotation;
        forward = player.transform.TransformDirection(Vector3.forward);
    }

    void FixedUpdate()
    {
        gameObject.transform.position +=
            forward *
            speed *
            Time.deltaTime
        ;
    }
}
