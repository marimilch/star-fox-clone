using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXHandler : MonoBehaviour
{
    public static FXHandler SharedComponent;

    // Start is called before the first frame update
    void Start()
    {
        SharedComponent = this;

        //set all inactive first
        var len = transform.childCount;
        for (int i = 0; i < len; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ShowFX(string location)
    {
        var loc = transform.Find(location);

        if (loc)
        {
            loc.gameObject.SetActive(true);
        } else
        {
            Debug.Log("Could not find damage location.");
        }
    }
}
