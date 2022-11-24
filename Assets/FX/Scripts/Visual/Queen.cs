using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AntFarm
{
    public class Queen : MonoBehaviour
    {
        public Animator anim;
        void Start()
        {
            Animation();
        }
        private void Animation()
        {
            anim.SetTrigger("Idle");
            float angl = Random.Range(-90f, 90f);
            anim.transform.DOLocalRotate(Vector3.up * angl, Mathf.Abs(angl) * 0.01f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuad)
                .SetDelay(Random.Range(1f, 3f))
                .OnStart(() => { anim.SetTrigger("Run"); })
                .OnComplete(() => { Animation(); });
        }


    }
}
