using UnityEngine;
using System.Collections;  // Добавляем это пространство имен для использования IEnumerator

public class RoofToggle : MonoBehaviour
{
    public GameObject roof;  // Объект крыши
    public float fadeDuration = 1f;  // Длительность эффекта исчезновения
    private Material roofMaterial;  // Материал крыши
    private Color originalColor;  // Оригинальный цвет крыши

    void Start()
    {
        // Получаем материал крыши
        roofMaterial = roof.GetComponent<Renderer>().material;
        originalColor = roofMaterial.color;  // Сохраняем оригинальный цвет крыши
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Начинаем плавное исчезновение крыши
            StartCoroutine(FadeOutRoof());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Начинаем плавное возвращение крыши
            StartCoroutine(FadeInRoof());
        }
    }

    IEnumerator FadeOutRoof()
    {
        float elapsedTime = 0f;
        Color color = roofMaterial.color;

        // Плавно уменьшаем альфа-канал цвета материала
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            roofMaterial.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Отключаем крышу после полного исчезновения
        roof.SetActive(false);
    }

    IEnumerator FadeInRoof()
    {
        roof.SetActive(true);  // Включаем крышу
        float elapsedTime = 0f;
        Color color = roofMaterial.color;

        // Плавно увеличиваем альфа-канал цвета материала
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, originalColor.a, elapsedTime / fadeDuration);
            roofMaterial.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
}
