using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance Instance { get; private set; }

    // Посилання на всі скрипти Player-а
    public PlayerController Controller { get; private set; }
    public PlayerHide Hide { get; private set; }
    public PlayerClothing Clothing { get; private set; }

    private void Awake()
    {
        Instance = this;
        Controller = GetComponent<PlayerController>();
        Hide = GetComponent<PlayerHide>();
        Clothing = GetComponent<PlayerClothing>();
    }
}
