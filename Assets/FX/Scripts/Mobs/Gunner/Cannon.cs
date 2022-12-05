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
        // public Transform target;
        public float speed = 1f;
        public Mob mob;

        void Start()
        {
            ball_p.gameObject.SetActive(false);
        }

        public void Shoot(Vector3 position)
        {
            ball_p.gameObject.SetActive(true);
            // target.parent.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            float dist = Vector3.Distance(transform.position, position);
            float t = dist / speed;
            ball_p.position = transform.position;
            ball_p.DOMove(position, t).SetEase(Ease.Linear).OnComplete(() => { ball_p.gameObject.SetActive(false); });

            Sequence sequence = DOTween.Sequence();
            sequence.Append(ball.DOLocalMoveY(dist * 0.3f, t / 2f).SetEase(Ease.OutQuad));
            sequence.Append(ball.DOLocalMoveY(0f, t / 2f).SetEase(Ease.InQuad));

        }
    }
}
