using System.Collections;
using UnityEngine;

namespace WizardsCode.MinDiab.FX
{
    /// <summary>
    /// Looks for particle systems in children of this object and waits for them to finish. 
    /// Once all particle effects have completed this parent object will be destroyed.
    /// </summary>
    public class DestroyAfterParticles : MonoBehaviour
    {
        [SerializeField, Tooltip("How long after the particles have finished should this object be destroyed?")]
        float m_Delay = 1;

        ParticleSystem[] particles;

        private void Start()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
            StartCoroutine(WaitForFinish());
        }

        IEnumerator WaitForFinish()
        {
            yield return null;
            bool finished = particles.Length == 0;

            while (!finished)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    if (!particles[i].IsAlive())
                    {
                        finished = true;
                        break;
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }

            yield return new WaitForSeconds(1f);

            if (finished)
            {
                Destroy(gameObject);
            }
        }
    }
}