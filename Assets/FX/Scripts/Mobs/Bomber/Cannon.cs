using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AntFarm
{
    public class Cannon : MonoBehaviour
    {
        public Transform ball_p;
        public Transform ball;
        public Transform target;
        public float speed=1f;
        public ParticleSystem shoot_FX;
        public ParticleSystem ball_FX;
        void Start()
        {
            ball_p.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(false);
            ball_FX.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Shoot()
        {
            shoot_FX.gameObject.SetActive(false);
            shoot_FX.gameObject.SetActive(true);
            ball_p.gameObject.SetActive(true);
            target.parent.forward = new Vector3( transform.forward.x,0, transform.forward.z);
            float dist = Vector3.Distance(transform.position, target.position);
            float t = dist / speed;
            ball_p.position = transform.position;
            ball_p.DOMove(target.position, t).SetEase(Ease.Linear).OnComplete(()=> { 
                ball_p.gameObject.SetActive(false);
                ball_FX.transform.position = target.transform.position;
                ball_FX.gameObject.SetActive(false);
                ball_FX.gameObject.SetActive(true);
            });

            Sequence sequence = DOTween.Sequence();
            sequence.Append(ball.DOLocalMoveY(dist*0.3f, t / 2f).SetEase(Ease.OutQuad));
            sequence.Append(ball.DOLocalMoveY(0f, t / 2f).SetEase(Ease.InQuad));

        }
    }
}
