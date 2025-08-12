using UnityEngine;

public class CameraFader : MonoBehaviour
{
    private ObjectFader fader;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null) {
                    return;
                }

                if (hit.collider.gameObject == player)
                {
                    if (fader != null)
                    {
                        fader.isFadeIn = false;
                    }
                }
                else
                {
                    fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (fader != null)
                    {
                        fader.isFadeIn = true;
                    }
                }
            }
        }
    }
}
