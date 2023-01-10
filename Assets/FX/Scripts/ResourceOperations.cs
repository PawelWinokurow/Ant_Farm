using UnityEngine;

public class ResourceOperations : MonoBehaviour
{
    private Store store;
    public PopUp popUp_prefad;
    public static ResourceOperations Instance { get; private set; }

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
        store = Store.Instance;
    }

    public void Buy(Vector3 position, int cost)
    {
        PopUp popUp = Instantiate(popUp_prefad, position, Quaternion.identity);
        popUp.tmp.text = (-cost).ToString();
        store.AddRemoveFood(-cost);
    }

    public void Sell(Vector3 position, int cost)
    {
        PopUp popUp = Instantiate(popUp_prefad, position, Quaternion.identity);
        popUp.tmp.text = '+' + cost.ToString();
        store.AddRemoveFood(cost);
    }
}
