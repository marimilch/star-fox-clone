using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObserver : MonoBehaviour
{
    Material initialMaterial;
    [SerializeField] Material whiteMaterial;
    new Renderer renderer;


    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        initialMaterial = renderer.material;
    }

    public void SetMaterial(string color)
    {
        if (color == "white")
        {
            renderer.material = whiteMaterial;
        } else if (color == "initial")
        {
            renderer.material = initialMaterial;
        }
    }
}
