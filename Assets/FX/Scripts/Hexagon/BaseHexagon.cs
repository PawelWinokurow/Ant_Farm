using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseHexagon : MonoBehaviour
{
    public static float Radius = 2f;
    public string Id { get; set; }
    public HEX_TYPE HexType { get; set; }
    public Vector3 Position { get; set; }

    public BaseHexagon AssignProperties(FoodHexagon hex)
    {
        Id = hex.Id;
        HexType = hex.HexType;
        Position = hex.Position;
        return this;
    }

    public static BaseHexagon CreateHexagon(string Id, BaseHexagon baseHexPrefab, Vector3 hexPosition, Transform parent, HEX_TYPE hexType)
    {
        BaseHexagon hex = Instantiate(baseHexPrefab, hexPosition, Quaternion.identity, parent);
        hex.Position = hexPosition;
        hex.Id = Id;
        hex.HexType = hexType;
        return hex;
    }

    public void ClearHexagon()
    {
        for (int i = 0; i < this.transform.childCount; i++)//удаляет чайлды старой графики
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}

