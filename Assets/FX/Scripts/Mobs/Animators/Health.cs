using System.Collections;
using AntFarm;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject deadFX_prefab;
    public GameObject hitFX_prefab;
    public MaterialPropertyBlock bodyProps;
    private MaterialPropertyBlock progressbarProps;
    public MeshRenderer progressbar;

    public float MAX_HP;
    public float hp;
    public float healTime;
    private float hitTime = 1000f;
    private bool isDead;
    public Color particlesColor;
    public Renderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        bodyProps = new MaterialPropertyBlock();
        progressbarProps = new MaterialPropertyBlock();
    }

    public void InitHp(float maxHP)
    {
        MAX_HP = maxHP;
        hp = maxHP;
        progressbar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        hitTime += Time.deltaTime / 0.2f;
        if (hitTime < 1f)//������� ��� �����
        {
            if(hitFX_prefab != null)
            {
                if (!isDead)
                {
                    renderers[0].transform.localScale = Vector3.one * ExtensionMethods.RemapClamp(Mathf.Min(1f, hitTime) / 0.1f, 0f, 1f, 1.2f, 1f);//���������
                }
                bodyProps.SetFloat("_Blink_FX", ExtensionMethods.RemapClamp(Mathf.Min(1f, hitTime), 0f, 1f, 0.7f, 0f));//�������

                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].SetPropertyBlock(bodyProps);
                }
            }


        }

        if (hp < MAX_HP)
        {
            progressbar.transform.localScale = new Vector3(MAX_HP / 100f, 1f, 1f);

            if (hitTime > 5f)//����������� ���� ��� ������� �� ���� �����
            {
                hp += Time.deltaTime * 100f / healTime;
            }
        }
        progressbar.enabled = !isDead && hp < MAX_HP;//��� ����������� ���������� �����������



        progressbarProps.SetFloat("_Health", ExtensionMethods.RemapClamp(hp / MAX_HP, 0f, 1f, 0.1f, 1f));
        progressbar.SetPropertyBlock(progressbarProps);
    }

    public void Restart()//test
    {
        renderers[0].gameObject.SetActive(true);
        isDead = false;
        hp = MAX_HP;
        renderers[0].transform.localScale = Vector3.one;
    }
    public float Hit(int damage)
    {
        if (!isDead)
        {
            hp -= damage;
            if (hitFX_prefab != null)
            {
                ColorFX colorFX = FX_Manager.instance.SpawnFromPool(hitFX_prefab, transform.position, transform.rotation).GetComponent<ColorFX>();
                colorFX.Colorize(particlesColor);
            }

            if (hp <= 0)
            {
                isDead = true;
                StartCoroutine(Dead_Cor());
            }
            hitTime = 0f;
        }
        return hp;
    }
    private IEnumerator Dead_Cor()
    {
        ColorFX colorFX = FX_Manager.instance.SpawnFromPool(deadFX_prefab, transform.position, transform.rotation).GetComponent<ColorFX>();
        colorFX.Colorize(particlesColor);
        renderers[0].transform.localScale = Vector3.one * 1.3f;
        yield return new WaitForSeconds(0.1f);
        renderers[0].gameObject.SetActive(false);
    }
}

