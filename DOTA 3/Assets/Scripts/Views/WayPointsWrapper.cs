using UnityEngine;

namespace Views
{
    public class WayPointsWrapper : MonoBehaviour
    {
        [SerializeField] private WayPoint _topLineWayPoint;
        [SerializeField] private WayPoint _midLineWayPoint;
        [SerializeField] private WayPoint _bottomLineWayPoint;
        
        public WayPoint TopLineWayPoint => _topLineWayPoint;
        public WayPoint MidLineWayPoint => _midLineWayPoint;
        public WayPoint BottomLineWayPoint => _bottomLineWayPoint;
    }
}