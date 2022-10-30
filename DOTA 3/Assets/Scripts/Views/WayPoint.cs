using System;
using UnityEngine;

namespace Views
{
    public class WayPoint : MonoBehaviour
    {
        [SerializeField] private WayPoint _nextLeftWayPoint;
        [SerializeField] private WayPoint _nextRightWayPoint;

        public WayPoint NextLeftWayPoint => _nextLeftWayPoint;
        public WayPoint NextRightWayPoint => _nextRightWayPoint;
    }
}