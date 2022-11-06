using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AnimationsScriptableObject ", order = 1)]
public class AnimationsScriptableObject : ScriptableObject
{
    public Material[] materials;
    public Mesh[] sequence;

}