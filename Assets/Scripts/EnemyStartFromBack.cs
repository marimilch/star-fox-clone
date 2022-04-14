using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartFromBack : MonoBehaviour, ICustomEnabler
{
    [SerializeField] float distance = -10f;
    [SerializeField] float dieDistance = -50f;

    bool hasSpawned = false;

    ComeFront comeFront;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    void SetComeFront()
    {
        comeFront = GetComponent<ComeFront>();
        comeFront.enabled = false;
        if (comeFront == null)
        {
            Debug.LogError("Enemy misses ComeFront for EnemStartFromBack");
            throw new MissingComponentException();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void RequiredInactiveStarts()
    {
        SetComeFront();
    }


    public bool CheckIfEnable()
    {
        if (
            !hasSpawned &&
            transform.position.z < distance
        )
        {
            hasSpawned = true;
            return true;
        }

        if (hasSpawned && transform.position.z > dieDistance)
        {
            return true;
        }

        comeFront.MakeMove();
        return false;
    }
}
