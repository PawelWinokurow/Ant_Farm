using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    public GameSettings gameSettings;
    public WorkerSettings workerSettings;
    public SoldierSettings soldierSettings;
    public GunnerSettings gunnerSettings;
    public ScorpionSettings scorpionSettings;
    public GobberSettings gobberSettings;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}