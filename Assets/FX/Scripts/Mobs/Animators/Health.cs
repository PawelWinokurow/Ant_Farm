using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public MeshRenderer mr;

    public ParticleSystemRenderer deadFX_prefab;
    public HealthPopUp popUp_prefad;

    MaterialPropertyBlock bodyProps;
    MaterialPropertyBlock progressbarProps;
    public MeshRenderer progressbar;


    public float MAX_HEALTH;
    public float health;
    public float healTime;
    private float hitTime = 1000f;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        bodyProps = new MaterialPropertyBlock();
        progressbarProps = new MaterialPropertyBlock();
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        hitTime += Time.deltaTime;

        if (hitTime <= 0.2f)//������� ��� �����
        {
            if (!isDead)
            {
                mr.transform.localScale = Vector3.one * ExtensionMethods.RemapClamp(hitTime / 0.1f, 0f, 1f, 1.2f, 1f);//���������
            }
            bodyProps.SetFloat("_Blink_FX", ExtensionMethods.RemapClamp(hitTime, 0f, 0.1f, 1f, 0f));//�������
            mr.SetPropertyBlock(bodyProps);

        }

        if (health < MAX_HEALTH)
        {
            progressbar.transform.localScale = new Vector3(MAX_HEALTH / 100f, 1f, 1f);

            if (hitTime > 2f)//����������� ���� ��� ������� �� ���� �����
            {
                health += Time.deltaTime * MAX_HEALTH / healTime;
            }
        }
        progressbar.enabled = !isDead && health < MAX_HEALTH;//��� ����������� ���������� �����������



        progressbarProps.SetFloat("_Health", ExtensionMethods.RemapClamp(health / MAX_HEALTH, 0f, 1f, 0.1f, 1f));
        progressbar.SetPropertyBlock(progressbarProps);
    }

    public void Restart()//test
    {
        mr.enabled = true;
        isDead = false;
        health = MAX_HEALTH;
        mr.transform.localScale = Vector3.one;
    }
    public void Hit(int damage)
    {
        health -= damage;
        if (health > 0)//���� ����
        {

            HealthPopUp healthPopUp = Instantiate(popUp_prefad, transform.position, transform.rotation, transform);//��������� ����� � ��������
            healthPopUp.Hit(damage);
        }
        else
        {
            if (!isDead)
            {
                isDead = true;
                StartCoroutine(Dead_Cor());
            }
        }
        hitTime = 0f;
    }
    private IEnumerator Dead_Cor()
    {
        ParticleSystemRenderer rend = Instantiate(deadFX_prefab, transform.position, transform.rotation);
        rend.material.SetColor("_Color", mr.material.GetColor("_Color"));
        rend.material.SetColor("_Color1", mr.material.GetColor("_Color1"));
        mr.transform.localScale = Vector3.one * 1.3f;
        yield return new WaitForSeconds(0.1f);
        mr.enabled = false;
    }
}

