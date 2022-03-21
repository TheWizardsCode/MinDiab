using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using WizardsCode.MinDiab.Character;
using WizardsCode.MinDiab.Combat;
using WizardsCode.MinDiab.Core;
using WizardsCode.MinDiab.Stats;

namespace WizardsCode.MinDiab.Controller
{
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(Scheduler))]
    [RequireComponent(typeof(Animator))]
    public partial class CharacterRoleController : MonoBehaviour
    {

        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Vector2 hotspot;
            public Texture2D texture;
        }

        [SerializeField, Tooltip("The maximum length a character will travel on the navmesh in a single movement.")]
        float m_maxNavMeshPathLength = 40;
        [SerializeField, Tooltip("The cursors to use for the various command options.")]
        CursorMapping[] m_CursorMappings = null;

        [Header("AI Influencer")]
        [SerializeField, Tooltip("The range within which this AI will alert other AIs when aggravated.")]
        float m_AggroRange = 5;
        [SerializeField, Tooltip("Once aggravated the character will take this amount of time to cooldown and return to a non-aggragated state,")]
        float m_AggravationCooldown = 5;

        internal Animator animator;
        internal Experience experience;
        internal Fighter fighter;
        internal HealthController health;
        internal MoveController mover;
        internal Scheduler scheduler;
        internal BaseStats stats;
        RuntimeAnimatorController m_DefaultAnimationController;
        float m_AggravatedUntil;

        public bool IsDead => health.IsDead;

        public bool IsAggravated
        {
            get
            {
                return m_AggravatedUntil > Time.timeSinceLevelLoad;
            }
            set
            {
                if (value)
                {
                    m_AggravatedUntil = Time.timeSinceLevelLoad + m_AggravationCooldown;
                }
                else
                {
                    m_AggravatedUntil = 0;
                }
            }
        }

        Camera mainCamera;
        public Camera MainCamera
        {
            get
            {
                if (!mainCamera)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }

        internal virtual void Awake()
        {
            animator = GetComponent<Animator>();
            m_DefaultAnimationController = animator.runtimeAnimatorController;
            fighter = GetComponent<Fighter>();
            experience = GetComponent<Experience>();
            health = GetComponent<HealthController>();
            mover = GetComponent<MoveController>();
            scheduler = GetComponent<Scheduler>();
            stats = GetComponent<BaseStats>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (HandleMovementInput()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        internal float GetStat(Stat stat, float baseValue = 0)
        {
            return stats.GetStat(stat, experience.Level, baseValue);
        }

        public void AlertNearbyAI()
        {
            // OPTIMIZATION: Use layers
            Collider[] candidates = Physics.OverlapSphere(transform.position, m_AggroRange);
            for (int i = 0; i < candidates.Length; i++)
            {
                AIController ai = candidates[i].GetComponent<AIController>();
                if (ai)
                {
                    ai.IsAggravated = true;
                }
            }
        }

        public void ResetAnimatorController()
        {
            animator.runtimeAnimatorController = m_DefaultAnimationController;
        }

        RaycastHit[] hits = new RaycastHit[5];

        internal void AddExperience(float amount)
        {
            experience.Add(amount);
        }



        private bool InteractWithComponent()
        {
            Array.Clear(hits, 0, hits.Length);
            Physics.RaycastNonAlloc(GetScreenPoint(), hits, 100, ~0, QueryTriggerInteraction.Collide);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == null) continue;

                // OPTIMIZATION: cache the results of this GetComponents
                IRaycastable[] raycastables = hits[i].transform.GetComponents<IRaycastable>();
                for (int y = 0; y < raycastables.Length; y++)
                {
                    if (raycastables[y].HandleRaycast(this))
                    {
                        SetCursor(raycastables[y].CursorType);
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            for (int i = 0; i < m_CursorMappings.Length; i++)
            {
                if (m_CursorMappings[i].type == type)
                {
                    return m_CursorMappings[i];
                }
            }
            Debug.LogError($"Attempting to get a cursor type {type} but none found. Returning default of {CursorType.None}");
            return GetCursorMapping(CursorType.None);
        }

        internal void StopAllActions()
        {
            scheduler.StopCurrentAction();
        }

        /// <summary>
        /// Move the player to the position of the cursor, if possible.
        /// </summary>
        /// <returns>True if the player has clicked to move, is able to move to the cursor, and has started movement. Otherwise return false.</returns>
        private bool HandleMovementInput()
        {
            NavMeshHit navHit;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetScreenPoint(), out hit);
            if (hasHit)
            {
                if (NavMesh.SamplePosition(hit.point, out navHit, 0.5f, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (!NavMesh.CalculatePath(transform.position, navHit.position, NavMesh.AllAreas, path)) return false;
                    if (path.status != NavMeshPathStatus.PathComplete) return false;
                    if (GetPathLengthSqr(path) > m_maxNavMeshPathLength * m_maxNavMeshPathLength) return false;

                    SetCursor(CursorType.Movement);
                    if (Input.GetMouseButton(0))
                    {
                        fighter.StopAction();
                        mover.MoveTo(navHit.position, 1);
                    }
                    return true;
                }
            }

            return false;
        }

        private float GetPathLengthSqr(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += (path.corners[i] - path.corners[i + 1]).sqrMagnitude;
            }
            return total;
        }

        /// <summary>
        /// Get a Ray from the camera to the screenpoint.
        /// </summary>
        /// <returns></returns>
        private Ray GetScreenPoint()
        {
            Debug.DrawRay(MainCamera.ScreenPointToRay(Input.mousePosition).origin, MainCamera.ScreenPointToRay(Input.mousePosition).direction, Color.red, 1);
            return MainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
