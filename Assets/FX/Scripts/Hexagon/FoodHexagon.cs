using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodHexagon : MonoBehaviour
{
    public static float Radius = 2f;
    public string Id { get; set; }
    public HEX_TYPE HexType { get; set; }
    public Vector3 Position { get; set; }
    public float Food { get; set; }

    public FoodHexagon AssignProperties(FoodHexagon hex)
    {
        Id = hex.Id;
        HexType = hex.HexType;
        Position = hex.Position;
        HexType = hex.HexType;
        Food = hex.Food;
        return this;
    }

    public static FoodHexagon CreateHexagon(string Id, FoodHexagon foodHexPrefab, Vector3 hexPosition, Transform parent)
    {
        FoodHexagon hex = Instantiate(foodHexPrefab, hexPosition, Quaternion.identity, parent);
        hex.Position = hexPosition;
        hex.Id = Id;
        hex.HexType = HEX_TYPE.FOOD;
        hex.Food = 1000f;
        return hex;
    }

    public void Clear()
    {
        for (int i = 0; i < this.transform.childCount; i++)//удаляет чайлды старой графики
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}

