//using UnityEngine;

//[ExecuteInEditMode]
//public class PlayerShaderPropertiesSetter : MonoBehaviour
//{
//    //SP = Shader property
//    private static int SP_PlayerWorldPosition = Shader.PropertyToID("_PlayerWorldPos");
//    private Transform _transform;

//    private void Start()
//    {
//        _transform = transform;
//    }

//    private void Update()
//    {
//        Shader.SetGlobalVector(SP_PlayerWorldPosition, _transform.position);
//    }
//}

using UnityEngine;

public class PlayerShaderPropertiesSetter : MonoBehaviour
{
    public Transform player;
    public Material obstacleMaterial;
    public float holeRadius = 2f;

    void Update()
    {
        Vector3 playerPos = player.position;
        obstacleMaterial.SetVector("_HolePosition", new Vector4(playerPos.x, playerPos.y, playerPos.z, 0));
        obstacleMaterial.SetFloat("_HoleRadius", holeRadius);
    }
}
