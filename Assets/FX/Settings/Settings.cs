using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    public GameSettings gameSettings;
    public WorkerSettings workerSettings;
    public FighterSettings soldierSettings;
    public FighterSettings gunnerSettings;
    public FighterSettings scorpionSettings;
    public FighterSettings gobberSettings;
    public FighterSettings zombieSettings;
    public FighterSettings blobSettings;
    public TrapSettings spikesSettings;
    public TrapSettings turretSettings;
    public PriceSettings priceSettings;
    public ResourcesSettings resourcesSettings;
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