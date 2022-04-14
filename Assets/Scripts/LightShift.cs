using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShift : MonoBehaviour
{
    [SerializeField] Color color1 = new Color(1f, 0f, 0f);
    [SerializeField] Color color2 = new Color(1f, 0.725f, 0f);
    [SerializeField] float transitionSpeed = 5f;
    //[SerializeField] Shader shader;

    float t = 0f;
    int direction = 1;
    new Renderer renderer;
    Material material;

    private void Awake()
    {
        material = new Material(Shader.Find("Unlit/Color"));
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t += direction * Time.deltaTime * transitionSpeed;

        //not using modulo on purpose so it always starts at 0f.
        if (t > 1f || t < 0f)
        {
            direction *= -1;
        }

        var currentColor = t * color1 + (1f - t) * color2;
        renderer.material = material;

        //blinking resets to preset material, so reset to this one
        material.color = currentColor;

    }
}
