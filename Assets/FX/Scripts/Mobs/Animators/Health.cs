using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Health : MonoBehaviour
    {
        public MeshRenderer mr;

        public GameObject deadFX_prefab;
        public HealthPopUp popUp_prefad;

        MaterialPropertyBlock bodyProps;
        MaterialPropertyBlock progressbarProps;
        public MeshRenderer progressbar;


        public float healthMax;
        public float health;
        public float healTime;
        private float hitTime = 1000f;
        private bool isDead;

        // Start is called before the first frame update
        void Start()
        {
            bodyProps = new MaterialPropertyBlock();
            progressbarProps = new MaterialPropertyBlock();
            health = healthMax;
        }

        // Update is called once per frame
        void Update()
        {
            hitTime += Time.deltaTime;

            if (hitTime <= 0.2f)//мигание при ударе
            {
                if (!isDead)
                {
                    mr.transform.localScale = Vector3.one * ExtensionMethods.RemapClamp(hitTime / 0.1f, 0f, 1f, 1.2f, 1f);//пульсация
                }
                    bodyProps.SetFloat("_Blink_FX", ExtensionMethods.RemapClamp(hitTime, 0f, 0.1f, 1f, 0f));//мигание
                    mr.SetPropertyBlock(bodyProps);
              
            }

            if (health < healthMax)
            {
                progressbar.transform.localScale = new Vector3(healthMax / 100f, 1f, 1f);

                if (hitTime > 2f)//оздоровляем если две секунды не было драки
                {
                    health += Time.deltaTime * healthMax / healTime;
                }
            }
            progressbar.enabled = !isDead && health < healthMax;//при повреждении показываем прогрессбар



            progressbarProps.SetFloat("_Health", ExtensionMethods.RemapClamp(health / healthMax, 0f, 1f, 0.1f, 1f));
            progressbar.SetPropertyBlock(progressbarProps);
        }

        public void Restart()//test
        {
            mr.enabled = true;
            isDead = false;
            health = healthMax;
            mr.transform.localScale = Vector3.one;
        }
        public void Hit(int damage)
        {
            health -= damage;
            if (health > 0)//пока живы
            {

                HealthPopUp healthPopUp = Instantiate(popUp_prefad, transform.position, transform.rotation);//инстантим попап с циферкой
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

            GameObject fx = Instantiate(deadFX_prefab, transform.position, transform.rotation);
            mr.transform.localScale = Vector3.one * 1.3f;
            yield return new WaitForSeconds(0.15f);
            mr.enabled = false;
        }
    }
}
