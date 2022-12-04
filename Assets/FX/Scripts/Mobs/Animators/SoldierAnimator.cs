using SoldierNamespace;
using UnityEngine;


public class SoldierAnimator : MonoBehaviour
{
    public AnimationsScriptableObject current;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject run;

    public MeshRenderer mr;

    private void Start()
    {
        current = run;
    }

    public void Run()
    {
        current = run;
        mr.materials = current.materials;
    }

    public void Idle()
    {
        current = idle;
        mr.materials = current.materials;
    }

}

