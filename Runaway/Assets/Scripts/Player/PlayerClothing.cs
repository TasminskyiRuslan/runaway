using UnityEngine;

public class PlayerClothing : MonoBehaviour
{
    public static PlayerClothing Instance;

    [Header("Clothing Objects")]
    [SerializeField] private GameObject _underwear;
    [SerializeField] private GameObject _shirt;
    [SerializeField] private GameObject _pants;
    [SerializeField] private GameObject _shoes;

    void Awake()
    {
        Instance = this;

        _underwear.SetActive(false);
        _shirt.SetActive(false);
        _pants.SetActive(false);
        _shoes.SetActive(false);
    }

    public void Equip(ClothingType type)
    {
        switch (type)
        {
            case ClothingType.Underwear: _underwear.SetActive(true); break;
            case ClothingType.Shirt: _shirt.SetActive(true); break;
            case ClothingType.Pants: _pants.SetActive(true); break;
            case ClothingType.Shoes: _shoes.SetActive(true); break;
        }
    }
}
