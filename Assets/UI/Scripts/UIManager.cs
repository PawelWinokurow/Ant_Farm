using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;
    public TMP_Text money;
    public SpawnIcon[] spawnIcons;
    private float t;
    public float waveDelay = 10f;
    public Animator swordsAnim;
    public Animator antAnim;
    private bool isFight;
    public Storyteller storyteller;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public void EnemySpawnIcons(int[] number)
    {
        int n = 0;
        for (int i = 0; i < spawnIcons.Length; i++)
        {
            if (number[i] > 0)
            {
                spawnIcons[i].gameObject.SetActive(true);
                spawnIcons[i].Play(n * 0.2f, number[i]);
                n++;
            }
            else
            {
                spawnIcons[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetFoodAmount(float amount)
    {
        money.text = Mathf.Ceil(amount).ToString();
    }

    private void Update()
    {
        t += Time.deltaTime / waveDelay;
        swordsAnim.transform.localPosition = new Vector3(ExtensionMethods.RemapClamp(t, 0f, 1f, 230, 0), 0f, 0f);
        if (t > 1f && !isFight)
        {
            isFight = true;
            Fight();
        }
    }

    public void Fight()
    {
        t = 1f;
        swordsAnim.SetTrigger("Play");
        antAnim.speed = 0f;
        StartCoroutine(SwordsFight_Cor());
        storyteller.SpawnHole();
    }

    private IEnumerator SwordsFight_Cor()
    {
        yield return new WaitForSeconds(5f);

        t = 0f;
        isFight = false;
        swordsAnim.SetTrigger("Stop");
        antAnim.speed = 1f;
    }

}
