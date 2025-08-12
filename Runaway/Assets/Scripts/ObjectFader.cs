using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 5, fadeAmount = 0.5f;
    public bool isFadeIn = false;
    private float originalOpacity;
    private Material[] materials;

    void Start()
    {
        materials = GetComponent<Renderer>().materials;
        foreach (Material material in materials)
        {
            originalOpacity = material.color.a;
        }
    }

    void Update()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        foreach (Material material in materials)
        {
            Color currentColor = material.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            material.color = smoothColor;
        }
    }

    void FadeOut()
    {
        foreach (Material material in materials)
        {
            Color currentColor = material.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
            material.color = smoothColor;
        }
    }
}
