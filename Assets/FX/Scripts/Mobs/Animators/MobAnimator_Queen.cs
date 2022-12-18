using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AntFarm
{
    public class MobAnimator_Queen : MonoBehaviour, IMobAnimator
    {
        public Animator anim;
        private Tween tween;
        public Vector3 enemyPos;

        void Start()
        {
            Idle();
        }

        public void Run()
        {
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

        public void IdleFight()
        {
            tween.Kill();
            tween = anim.transform.DOLocalRotate(Vector3.zero, 0.3f, RotateMode.Fast).SetEase(Ease.OutQuad).OnStart(() => { anim.SetTrigger("Fight"); });
            /*
            tween.Kill();
            Quaternion look = Quaternion.LookRotation(anim.transform.parent.up, enemyPos - transform.position);
            look *= Quaternion.Euler(new Vector3(-90, 0, 180));
            tween = anim.transform.DORotateQuaternion(look, 0.3f).SetEase(Ease.OutQuad).OnStart(() => { anim.SetTrigger("Fight"); });
            */
        }
        public void RunFood()
        {
        }
        public void Shoot() 
        {
            Debug.Log("Shoot");
        }

    }
}
