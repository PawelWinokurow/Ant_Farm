using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AntFarm
{
    public class Queen : MonoBehaviour
    {
        public Animator anim;
        private Tween tween;

        void Start()
        {
            Idle();
        }
        public void Idle()
        {
            if (tween != null)
            {
                tween.Kill();
            }


            anim.SetTrigger("Idle");
            float angl = Random.Range(-90f, 90f);
            tween = anim.transform.DOLocalRotate(Vector3.up * angl, Mathf.Abs(angl) * 0.01f, RotateMode.LocalAxisAdd)
                  .SetEase(Ease.InOutQuad)
                  .SetDelay(Random.Range(1f, 3f))
                  .OnStart(() => { anim.SetTrigger("Run"); })
                  .OnComplete(() => { Idle(); });
        }

        public void Fight(Vector3 enemyPos)
        {
            tween.Kill();
            Quaternion look = Quaternion.LookRotation(anim.transform.parent.up, enemyPos - transform.position);
            look *= Quaternion.Euler(new Vector3(-90, 0, 180));
            tween = anim.transform.DORotateQuaternion(look, 0.3f).SetEase(Ease.OutQuad).OnStart(() => { anim.SetTrigger("Fight"); });

        }
        public void Shoot() 
        {
            Debug.Log("Shoot");
        }

    }
}
