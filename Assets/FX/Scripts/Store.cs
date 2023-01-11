using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    public List<Targetable> allAllies = new List<Targetable>();
    public List<Targetable> allEnemies = new List<Targetable>();
    public static Store Instance { get; private set; }
    public float food;
    public ResourcesSettings resourcesSettings;
    private static object monitorLock = new object();
    private TextMeshProUGUI moneyUI;

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

    private void Start()
    {
        resourcesSettings = Settings.Instance.resourcesSettings;
        food = resourcesSettings.INITIAL_FOOD_AMOUNT;
        UIManager.instance.SetFoodAmount(food);
        moneyUI = GameObject.FindGameObjectWithTag("MoneyUI").GetComponent<TextMeshProUGUI>();
    }

    public void AddRemoveFood(float amount)
    {
        lock (monitorLock)
        {
            food += amount;
            UIManager.instance.SetFoodAmount(Mathf.Ceil(food));
        }
    }

    public void AddAlly(Targetable mob)
    {
        lock (monitorLock)
        {
            allAllies.Add(mob);
        }
    }
    public void DeleteAlly(Targetable mob)
    {
        lock (monitorLock)
        {
            allAllies.Remove(mob);
        }
    }
    public void AddEnemy(Targetable mob)
    {
        lock (monitorLock)
        {
            allEnemies.Add(mob);
        }
    }
    public void DeleteEnemy(Targetable mob)
    {
        lock (monitorLock)
        {
            allEnemies.Remove(mob);
        }
    }
}

