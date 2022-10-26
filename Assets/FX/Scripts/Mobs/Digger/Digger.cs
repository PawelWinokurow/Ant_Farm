using UnityEngine;
using UnityEngine.AI;
public class Digge : MonoBehaviour, Mob
{
    public float damagePeriod = 0.3f;


    private DigJob job;

    private Path path;

    private void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }


    public bool HasJob()
    {
        return job != null;
    }

    public void SetPath(Path path)
    {
        this.path = path;
    }
    public void SetJob(Job job)
    {
        this.job = (DigJob)job;
    }





}
