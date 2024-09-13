using UnityEngine;
using Cinemachine;

public class CameraRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float rotationStep = 90f;
    private CinemachineOrbitalTransposer orbitalTransposer;

    void Start()
    {
        orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public void RotateCamera(bool clockwise)
    {
        if (clockwise)
        {
            orbitalTransposer.m_XAxis.Value += rotationStep;
        }
        else
        {
            orbitalTransposer.m_XAxis.Value -= rotationStep;
        }
    }
}
