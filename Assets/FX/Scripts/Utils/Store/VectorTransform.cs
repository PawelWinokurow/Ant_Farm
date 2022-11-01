using UnityEngine;

public class VectorTransform
{
    public static Vector3Serializable ToSerializable(Vector3 vec)
    {
        return new Vector3Serializable(vec.x, vec.y, vec.z);
    }
    public static Vector3 FromSerializable(Vector3Serializable vecSerializable)
    {
        return new Vector3(vecSerializable.x, vecSerializable.y, vecSerializable.z);
    }
}