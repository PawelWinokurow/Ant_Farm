using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AntFarm
{
    public class Shoot_Listener : MonoBehaviour
    {
        public UnityEvent m_Shoot;

        void Start()
        {
        
        }


       public  void Shoot()
        {
            m_Shoot.Invoke();
        }
    }
}
