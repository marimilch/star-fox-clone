using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Functional;

public class ActiveInBounds : MonoBehaviour
{
    [SerializeField] float backBound;
    [SerializeField] float frontBound;

    int childCount;

    // Start is called before the first frame update
    void Start()
    {
        childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            var child = transform.GetChild(i).gameObject;
            var maybeCustomEnabler = child.GetComponent<ICustomEnabler>();
            if (maybeCustomEnabler != null)
            {
                maybeCustomEnabler.RequiredInactiveStarts();
            }
        }
    }

    //void Map<T>(T[] elems, Each<T> each)
    //{
    //    for (int i = 0; i < elems.Length; ++i)
    //    {
    //        each(elems[i]);
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < childCount; ++i)
        {
            var child = transform.GetChild(i).gameObject;
            var maybeCustomEnabler = child.GetComponent<ICustomEnabler>();
            if (maybeCustomEnabler != null)
            {
                //object will handle its spawn itself
                var state = maybeCustomEnabler.CheckIfEnable();
                if (state != child.activeSelf)
                {
                    //Debug.Log("Set state to " + state);
                    child.SetActive(state);
                    if (child.activeSelf)
                    {
                        HandleReadyToShoot(child);
                    }
                }

                continue;
            }

            var zPos = child.transform.position.z;

            if (zPos < backBound && zPos > frontBound)
            {
                if (!child.activeSelf)
                {
                    child.SetActive(true);
                    HandleReadyToShoot(child);
                }
            } else
            {
                if (child.activeSelf)
                {
                    //ReadyToShoot();
                    child.SetActive(false);
                }
            }
        }
    }

    void HandleReadyToShoot(GameObject child)
    {
        var maybeShoots = child.GetComponent<EnemyShooting>();
        if (maybeShoots != null && child.activeSelf)
        {
            maybeShoots.ReadyToShoot();
        }
    }
}
