using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.MinDiab.Controller
{

    public class PatrolController : MonoBehaviour
    {
        [SerializeField, Tooltip("The average time to dwell at each waypoint.")]
        float m_AverageDwellTime = 0.5f;

        public float DwellTime {
            get
            {
                return m_AverageDwellTime * Random.Range(0.8f, 1.2f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            const float radius = 0.3f;

            Gizmos.color = Color.gray;
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 waypoint = GetWaypointPosition(i);
                waypoint.y += radius / 2;
                Gizmos.DrawSphere(waypoint, radius);

                Vector3 nextwaypoint;
                if (i == transform.childCount - 1)
                {
                    nextwaypoint = GetWaypointPosition(0);
                }
                else
                {
                    nextwaypoint = GetWaypointPosition(i + 1);
                }
                nextwaypoint.y += radius / 2;
                Gizmos.DrawLine(waypoint, nextwaypoint);
            }
        }

        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextWaypointIndex(int currentWaypointIndex)
        {
            if (currentWaypointIndex == transform.childCount - 1)
            {
                return 0;
            }
            else
            {
                return currentWaypointIndex + 1;
            }
        }
    }
}