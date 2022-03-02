using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Core
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField, Tooltip("Should this pickup be destroyed when collected?")]
        bool m_DestroyOnPickup = false;
        [SerializeField, Tooltip("If not destroyed on pickup how long should the pickup be hidden for after pickup?")]
        float m_TimeToHideOnPickup = 5;

        internal virtual void OnTriggerEnter(Collider other)
        {
            if (!CanPickup(other)) return; 

            StartCoroutine(DestroyOrHide());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Test to see if an object that has tried to pickup this item can do so.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal bool CanPickup(Collider other)
        {
            return other.CompareTag("Player");
        }

        private IEnumerator DestroyOrHide()
        {
            yield return null;

            if (m_DestroyOnPickup)
            {
                Destroy(gameObject);
            }

            GetComponent<Collider>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(m_TimeToHideOnPickup);

            GetComponent<Collider>().enabled = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}