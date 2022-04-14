using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    public static TunnelManager SharedComponent;

    Dictionary<string, int> tunnelCounter;

    // Start is called before the first frame update
    void Awake()
    {
        SharedComponent = this;
        tunnelCounter = new Dictionary<string, int>();
    }

    // Update is called once per frame
    public void IncreaseCounter(string key)
    {
        if (tunnelCounter.ContainsKey(key))
        {
            tunnelCounter[key] += 1;
        } else
        {
            tunnelCounter[key] = 1;
        }
        Debug.Log("Tunnel " + key + ": " + tunnelCounter[key]);
    }
}
