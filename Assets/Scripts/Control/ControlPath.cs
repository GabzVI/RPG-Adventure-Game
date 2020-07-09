using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class ControlPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.2f;

        private void OnDrawGizmos()
        {
            //childCount is the amount of children that is under the parent.
            for(int i =0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GetWayPointPos(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWayPointPos(i), GetWayPointPos(j));
            }
        }

        public int GetNextIndex(int i)
        {
            //This is done so that the last point reconects with the first point.
            if(i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

       public Vector3 GetWayPointPos(int i)
        {
            return transform.GetChild(i).position;
        }
    }

}
