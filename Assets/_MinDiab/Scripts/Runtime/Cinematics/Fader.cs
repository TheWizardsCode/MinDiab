using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Cinematics
{
    public class Fader : MonoBehaviour
    {
        static bool alreadySpawned = false;

        CanvasGroup group;

        void Awake()
        {
            if (alreadySpawned) return;

            group = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(group.gameObject);
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