using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GunnerNamespace
{
    public class Cannon : MonoBehaviour
    {
        public Transform ball_p;
        public Transform ball;
        public float speed = 1f;
        public ParticleSystem shoot_FX;
        public ParticleSystem ball_FX;
        public Gunner gunner;
        private Sequence sequence;
        private float a;
        private Vector3 targetPos;
        void Start()
        {
            ball_p.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(false);
            ball_FX.gameObject.SetActive(false);
        }

        public void Shoot()
        {
            if (sequence != null)
            {
                sequence.Complete();
                sequence.Kill();
            }
            shoot_FX.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(true);
            ball_p.gameObject.SetActive(true);
            if (gunner.target != null)
            {
                targetPos = gunner.target.mob.position;
            }
            float dist = Vector3.Distance(transform.position, targetPos);
            float t = dist / speed;
            ball_p.position = transform.position;
            ball.transform.localPosition = Vector3.zero;
            a = 0;
            sequence = DOTween.Sequence();

            sequence.Join(DOTween.To(() => a, x => a = x, 1f, t).SetEase(Ease.Linear).OnUpdate(() =>
            {
                if (gunner.target != null)
                {
                    targetPos = gunner.target.mob.position;
                }
                ball_p.transform.position = Vector3.Lerp(transform.position, targetPos, a);
            }
            ).OnComplete(() =>
       {
           ball_p.gameObject.SetActive(false);
           ball_FX.transform.position = targetPos;
           ball_FX.gameObject.SetActive(false);
           ball_FX.gameObject.SetActive(true);
           
           if (gunner.target?.mob.Hit(Settings.Instance.gunnerSettings.ATTACK_STRENGTH) <= 0)
           {
               gunner.CancelJob();
               gunner.SetState(new PatrolState(gunner));
           }
           
       }));

            sequence.Join(ball.DOLocalMoveY(dist * 0.3f, t / 2f).SetEase(Ease.OutQuad));
            sequence.Join(ball.DOLocalMoveY(0f, t / 2f).SetEase(Ease.InQuad).SetDelay(t / 2f));

        }
    }


}

