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
        public Transform target;
        public float speed = 1f;
        public ParticleSystem shoot_FX;
        public ParticleSystem ball_FX;
        public Gunner gunner;

        void Start()
        {
            ball_p.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(false);
            ball_FX.gameObject.SetActive(false);
        }

        public void Shoot()
        {
            shoot_FX.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(true);
            ball_p.gameObject.SetActive(true);
            // target.parent.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            float dist = Vector3.Distance(transform.position, gunner.target.mob.position);
            float t = dist / speed;
            ball_p.position = transform.position;
            float a = 0;
            DOTween.To(() => a, x => a = x, 1f, t)
                .OnUpdate(() => {
                    ball_p.transform.position=Vector3.Lerp(transform.position, gunner.target.mob.position, a);
                }).SetEase(Ease.Linear).OnComplete(() =>
            {
                ball_p.gameObject.SetActive(false);
                ball_FX.transform.position = gunner.target.mob.position;
                ball_FX.gameObject.SetActive(false);
                ball_FX.gameObject.SetActive(true);

                if (gunner.target.mob.Hit(Settings.Instance.gunnerSettings.ATTACK_STRENGTH) <= 0)
                {
                    gunner.CancelJob();
                    gunner.SetState(new PatrolState(gunner));
                }
            });

            Sequence sequence = DOTween.Sequence();
            sequence.Append(ball.DOLocalMoveY(dist * 0.3f, t / 2f).SetEase(Ease.OutQuad));
            sequence.Append(ball.DOLocalMoveY(0f, t / 2f).SetEase(Ease.InQuad));

        }
    }
}
