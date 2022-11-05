using UnityEngine;


public class WorkHexagon : MonoBehaviour, Hexagon
{
    public string Id { get; set; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }
    public Vector3 Position { get; set; }
    public FloorHexagon FloorHexagon { get; set; }
    public float Work { get; set; }


    public static WorkHexagon CreateHexagon(FloorHexagon parent, WorkHexagon workHexPrefab)
    {
        WorkHexagon hex = Instantiate(workHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.Position = parent.Position;
        hex.Id = parent.Id;
        hex.Work = parent.Work;
        hex.FloorHexagon.Child = hex;
        return hex;
    }

    public WorkHexagon AssignProperties(WorkHexagon hex)
    {
        Id = hex.Id;
        Type = hex.Type;
        Position = hex.Position;
        Work = hex.Work;
        return this;
    }

}

