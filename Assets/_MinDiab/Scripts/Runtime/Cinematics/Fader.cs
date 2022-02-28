using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Cinematics
{
    public class Fader : MonoBehaviour
    {
        static CanvasGroup group;

        void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Fade the screen out to the image color of the fader this is attached to
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeOut(float time)
        {
            while (group.alpha < 1)
            {
                group.alpha += Time.deltaTime / time;
                yield return null;
            }
        }


        /// <summary>
        /// Fade the screen in to full visibility.
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeIn(float time)
        {
            while (group.alpha > 0)
            {
                group.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}