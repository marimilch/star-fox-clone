using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SharedInstance;

    private Dictionary<string, List<GameObject>> pool;
    [SerializeField] private List<GameObject> toCreate;
    [SerializeField] private int numberOfInstances;

    // Start is called before the first frame update
    void Awake()
    {
        SharedInstance = this;
        pool = new Dictionary<string, List<GameObject>>();

        for (int j = 0; j < toCreate.Count; j++)
        {
            var g = toCreate[j];
            var gName = g.name;
            pool.Add(gName, new List<GameObject>(numberOfInstances));

            var emptyG = new GameObject();
            emptyG.name = g.name;
            emptyG.transform.SetParent(gameObject.transform);

            for (int i = 0; i < numberOfInstances; ++i)
            {
                
                var instance = Instantiate(
                    g, Vector3.zero, g.transform.rotation
                );

                instance.SetActive(false);

                instance.transform.SetParent(emptyG.transform);

                pool[gName].Add(instance);
            }
        }    
    }

    public GameObject GetInstance(string name)
    {
        for(int i = 0; i < numberOfInstances; ++i)
        {
            var g = pool[name][i];
            if (!g.activeSelf)
            {
                //var shootScript = pool[i].GetComponent<Shoot>();
                g.SetActive(true);
                return g;
            }
        }
        return null;
    }

    public List<GameObject> GetAllInstances(string name)
    {
        return pool[name];
    }
}
