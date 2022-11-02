using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AnimationsScriptableObject ", order = 1)]
public class AnimationsScriptableObject : ScriptableObject
{
    public float speed=1f;
    public Mesh[] sequence;

}