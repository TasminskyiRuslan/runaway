using UnityEngine;
using Cinemachine;

public class CameraRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Посилання на Cinemachine Virtual Camera
    public float rotationStep = 90f; // Крок обертання
    public float rotationSpeed = 5f; // Швидкість плавного обертання
    private CinemachineOrbitalTransposer orbitalTransposer;
    private float targetAngle; // Цільовий кут

    void Start()
    {
        // Отримання посилання на Orbital Transposer
        orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        // Встановлення початкового кута
        targetAngle = orbitalTransposer.m_XAxis.Value;
    }

    void Update()
    {
        // Плавне обертання камери до цільового кута
        orbitalTransposer.m_XAxis.Value = Mathf.LerpAngle(orbitalTransposer.m_XAxis.Value, targetAngle, Time.deltaTime * rotationSpeed);
    }

    public void RotateCamera(bool clockwise)
    {
        if (clockwise)
        {
            targetAngle += rotationStep;
        }
        else
        {
            targetAngle -= rotationStep;
        }

        // Нормалізація кута між 0 і 360 градусами
        targetAngle = Mathf.Repeat(targetAngle, 360f);
    }
}
