using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<Hexagon> hexagons;

    public Hexagon hexagonFloor;
    public Hexagon hexagonWall;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

    }
   public void AddFloor(Hexagon hex)
    {
        Vector3 pos = hex.transform.position;
        hexagons.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        hexagons.Add(Instantiate(hexagonFloor, pos, Quaternion.identity));
    }

   
}
