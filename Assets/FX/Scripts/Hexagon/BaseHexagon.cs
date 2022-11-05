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
        HexType = hex.HexType;
        return this;
    }

    public static BaseHexagon CreateHexagon(string Id, BaseHexagon baseHexPrefab, Vector3 hexPosition, Transform parent)
    {
        BaseHexagon hex = Instantiate(baseHexPrefab, hexPosition, Quaternion.identity, parent);
        hex.Position = hexPosition;
        hex.Id = Id;
        hex.HexType = HEX_TYPE.BASE;
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

