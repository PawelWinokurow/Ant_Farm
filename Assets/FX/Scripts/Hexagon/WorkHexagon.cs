using UnityEngine;


public class WorkHexagon : MonoBehaviour, Hexagon
{
    public string id { get => floorHexagon.id; set => floorHexagon.id = value; }
    public Vector3 position { get => floorHexagon.position; set => floorHexagon.position = value; }
    public HEX_TYPE type { get => floorHexagon.type; set => floorHexagon.type = value; }
    public FloorHexagon floorHexagon { get; set; }
    public float work { get; set; }
    public static float MAX_WORK = 100f;


    public static WorkHexagon CreateHexagon(FloorHexagon parent, WorkHexagon workHexPrefab)
    {
        WorkHexagon hex = Instantiate(workHexPrefab, parent.position, Quaternion.identity, parent.transform);
        hex.floorHexagon = parent;
        hex.floorHexagon.child = hex;
        hex.work = MAX_WORK;
        return hex;
    }

    public WorkHexagon AssignProperties(WorkHexagon hex)
    {
        work = hex.work;
        return this;
    }

}

