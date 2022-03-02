using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.FX
{
    public class HitEffect : MonoBehaviour
    {
        ParticleSystem[] particles;

        private void Start()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        void Update()
        {
            bool finished = false;

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].IsAlive())
                {
                    finished = true;
                    break;
                }
            }

            if (finished)
            {
                Destroy(gameObject);
            }
        }
    }
}