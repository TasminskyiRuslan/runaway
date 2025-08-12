using UnityEngine;
using System.Collections.Generic;

public class CameraObstructionHandler : MonoBehaviour
{
    public Transform player; // Гравець (або його голова)
    public LayerMask obstructionMask; // Шар об'єктів, які можуть заважати
    public float checkDistance = 5f; // Дистанція перевірки
    public float fadeSpeed = 5f; // Швидкість зникнення
    public float targetAlpha = 0.3f; // Бажана прозорість (0-1)

    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();
    private List<GameObject> currentlyTransparent = new List<GameObject>();

    void Update()
    {
        Vector3 cameraPos = transform.position;
        Vector3 playerPos = player.position;
        Vector3 direction = playerPos - cameraPos;

        // Очищаємо попередні прозорі об'єкти
        ResetPreviousTransparency();

        // Випускаємо Raycast для виявлення перешкод
        RaycastHit[] hits = Physics.RaycastAll(cameraPos, direction, checkDistance, obstructionMask);

        foreach (RaycastHit hit in hits)
        {
            GameObject obj = hit.collider.gameObject;

            // Якщо об'єкт ще не прозорий, зберігаємо його матеріали
            if (!originalMaterials.ContainsKey(obj))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    originalMaterials[obj] = renderer.materials;
                }
            }

            // Робимо об'єкт прозорим
            ApplyTransparency(obj, targetAlpha);
            currentlyTransparent.Add(obj);
        }
    }

    private void ApplyTransparency(GameObject obj, float alpha)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = new Material(Shader.Find("Custom/TransparentShader"));
                newMaterials[i].color = originalMaterials[obj][i].color;
                newMaterials[i].SetFloat("_Transparency", alpha);
            }
            renderer.materials = newMaterials;
        }
    }

    private void ResetPreviousTransparency()
    {
        foreach (GameObject obj in currentlyTransparent)
        {
            if (obj != null && originalMaterials.ContainsKey(obj))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.materials = originalMaterials[obj];
                }
            }
        }
        currentlyTransparent.Clear();
    }
}