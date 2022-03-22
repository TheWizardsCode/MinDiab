using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.MinDiab.Controller;

namespace WizardsCode.MinDiab.Core
{
    [RequireComponent(typeof(Collider))]
    public class BasePickup : MonoBehaviour, IRaycastable
    {
        [SerializeField, Tooltip("If this is true then the item will be picked up whenever it is clicked on, " +
            "the character does not need to move to the item to pick it up.")]
        bool m_PickupOnClick = false;
        [SerializeField, Tooltip("Should this pickup be destroyed when collected?")]
        bool m_DestroyOnPickup = false;
        [SerializeField, Tooltip("If not destroyed on pickup how long should the pickup be hidden for after pickup?")]
        float m_TimeToHideOnPickup = 5;

        public CursorType CursorType { get { return CursorType.Interactable; } }

        private void OnTriggerEnter(Collider other)
        {
            PickupItem(other.gameObject);
        }

        internal virtual void PickupItem(GameObject character)
        {
            if (!CanPickup(character)) return;
            StartCoroutine(DestroyOrHide());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Test to see if an charcter that has tried to pickup this item can do so.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        internal virtual bool CanPickup(GameObject character)
        {
            return character.CompareTag("Player");
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

        public bool HandleRaycast(CharacterRoleController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_PickupOnClick)
                {
                    PickupItem(controller.gameObject);
                } else
                {
                    controller.mover.MoveTo(transform.position, 1);
                }
            }
            return true;
        }
    }
}