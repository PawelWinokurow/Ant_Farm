using UnityEngine;
using WorkerNamespace;
public class WorkerAnimator : MonoBehaviour
{
    public AnimationsScriptableObject current;
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject runFood;

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
    public void RunFood()
    {
        current = runFood;
        mr.materials = current.materials;
    }
}