using UnityEngine;


public class WorkHexagon : MonoBehaviour, Hexagon
{
    public string Id { get => FloorHexagon.Id; set => FloorHexagon.Id = value; }
    public Vector3 Position { get => FloorHexagon.Position; set => FloorHexagon.Position = value; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }
    public FloorHexagon FloorHexagon { get; set; }
    public float Work { get; set; }
    public static float MaxWork = 100f;


    public static WorkHexagon CreateHexagon(FloorHexagon parent, WorkHexagon workHexPrefab)
    {
        WorkHexagon hex = Instantiate(workHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.FloorHexagon.Child = hex;
        hex.Work = MaxWork;
        return hex;
    }

    public WorkHexagon AssignProperties(WorkHexagon hex)
    {
        Work = hex.Work;
        return this;
    }

}

