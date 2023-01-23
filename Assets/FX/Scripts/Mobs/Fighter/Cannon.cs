using UnityEngine;
using DG.Tweening;
using TrapNamespace;

namespace FighterNamespace
{
    public class Cannon : MonoBehaviour
    {
        public Transform ball_p;
        public Transform ball;
        public float speed = 1f;
        public ParticleSystem shoot_FX;
        public ParticleSystem ball_FX;
        public Fighter fighter;
        public Trap trap;
        private Sequence sequence;
        private float a;
        private Vector3 targetPos;
        private Vector3 ballPosOld;
        public bool isStatic = false;
        void Start()
        {
            ball_p.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(false);
            ball_FX.gameObject.SetActive(false);
        }

        public void Shoot()
        {
            if (fighter?.currentState.type != STATE.DEAD || trap?.currentState.type != STATE.DEAD)
            {

                if (sequence != null)
                {
                    sequence.Complete();
                    sequence.Kill();
                }
                shoot_FX.gameObject.SetActive(false);
                shoot_FX.gameObject.SetActive(true);
                ball_p.gameObject.SetActive(true);
                if (isStatic)
                {
                    if (trap.target != null)
                    {
                        targetPos = trap.target.mob.position;
                    }
                }
                else
                {
                    if (fighter.target != null)
                    {
                        targetPos = fighter.target.mob.position;
                    }
                }

                float dist = Vector3.Distance(transform.position, targetPos);
                float t = dist / speed;
                ball_p.position = transform.position;
                ball.transform.localPosition = Vector3.zero;
                a = 0;
                sequence = DOTween.Sequence();

                sequence.Join(DOTween.To(() => a, x => a = x, 1f, t).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    if (isStatic)
                    {
                        if (trap.target != null)
                        {
                            targetPos = trap.target.mob.position;
                        }
                    }
                    else
                    {
                        if (fighter.target != null)
                        {
                            targetPos = fighter.target.mob.position;
                        }
                    }

                    ball_p.position = Vector3.Lerp(transform.position, targetPos, a);
                    ball.localPosition = Vector3.up * (1f - (1f - a * 2f) * (1f - a * 2f)) * dist * 0.3f;

                    ball_p.LookAt(targetPos, transform.up);
                    ball.LookAt(ballPosOld, Vector3.up);
                    ballPosOld = ball.position + ball.forward;
                }
                ).OnComplete(() =>
           {
               ball_p.gameObject.SetActive(false);
               ball_FX.transform.position = targetPos;
               ball_FX.gameObject.SetActive(false);
               ball_FX.gameObject.SetActive(true);

               //TODO remove Static
               if (isStatic)
               {
                   trap.Attack();
               }
               else
               {
                   fighter.Attack();
               }

           }));

            }
        }

    }
}

