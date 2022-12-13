using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GobberNamespace
{
    public class Cannon : MonoBehaviour
    {
        public Transform ball_p;
        public Transform ball;
        public float speed = 1f;
        public ParticleSystem shoot_FX;
        public ParticleSystem ball_FX;
        public Gobber gobber;
        private Sequence sequence;
        private float a;
        private Vector3 targetPos;
        private Vector3 ballPosOld;
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
            if (gobber.target != null)
            {
                targetPos = gobber.target.mob.position;
            }
            // else//test
            //{
            //targetPos = new Vector3(transform.position.x + Random.Range(-10f, 10f), 0, transform.position.z + Random.Range(-10f, 10f));//test
            //}

            float dist = Vector3.Distance(transform.position, targetPos);
            float t = dist / speed;
            ball_p.position = transform.position;
            ball.transform.localPosition = Vector3.zero;
            a = 0;
            sequence = DOTween.Sequence();

            sequence.Join(DOTween.To(() => a, x => a = x, 1f, t).SetEase(Ease.Linear).OnUpdate(() =>
            {
                if (gobber.target != null)
                {
                    targetPos = gobber.target.mob.position;
                }
                ball_p.transform.position = Vector3.Lerp(transform.position, targetPos, a);
                ball_p.LookAt(targetPos,transform.up);
                ball.LookAt(ballPosOld, Vector3.up);
                ballPosOld = ball.position;
            }
            ).OnComplete(() =>
       {
           ball_p.gameObject.SetActive(false);
           ball_FX.transform.position = targetPos;
           ball_FX.gameObject.SetActive(false);
           ball_FX.gameObject.SetActive(true);

           if (gobber.target?.mob.Hit(Settings.Instance.gobberSettings.ATTACK_STRENGTH) <= 0)
           {
               gobber.CancelJob();
               gobber.SetState(new PatrolState(gobber));
           }

       }));

            sequence.Join(ball.DOLocalMoveY(dist * 0.3f, t / 2f).SetEase(Ease.OutQuad));
            sequence.Join(ball.DOLocalMoveY(0f, t / 2f).SetEase(Ease.InQuad).SetDelay(t / 2f));

        }
    }


}

