using UnityEngine;


public class WorkHexagon : MonoBehaviour, Hexagon
{
    public string id { get => floorHexagon.id; set => floorHexagon.id = value; }
    public Vector3 position { get => floorHexagon.position; set => floorHexagon.position = value; }
    public HexType type { get => floorHexagon.type; set => floorHexagon.type = value; }
    public FloorHexagon floorHexagon { get; set; }
    public static float MAX_WORK = 100f;
    public float work = MAX_WORK;
    public bool isGroundMat;

    public static WorkHexagon CreateHexagon(FloorHexagon parent, WorkHexagon workHexPrefab)
    {
        WorkHexagon hex = Instantiate(workHexPrefab, parent.position, Quaternion.identity, parent.transform);
        hex.floorHexagon = parent;
        hex.floorHexagon.child = hex;
        hex.work = MAX_WORK;
        AddMaterial(parent);
        return hex;
    }
    public static void AddMaterial(FloorHexagon parent)
    {
        WorkHexagon[] workHexagons = parent.GetComponentsInChildren<WorkHexagon>();
        for (int i = 0; i < workHexagons.Length; i++)
        {
            if (workHexagons[i].isGroundMat == true)
            {
                workHexagons[i].GetComponent<MeshRenderer>().sharedMaterial = parent.mr.sharedMaterial;
            }
        }
    }

    public WorkHexagon AssignProperties(WorkHexagon hex)
    {
        work = hex.work;
        return this;
    }

}

