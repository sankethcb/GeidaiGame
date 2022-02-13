using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Raycaster : MonoBehaviour
    {
        public enum CastType
        {
            RAY,
            CIRCLE,
            RECT
        }

        [Header("Settings")]
        public Vector3 Direction;
        public float Distance = 0.1f;
        public LayerMask Mask;

        [Header("Results")]
        [SerializeField] [ReadOnly] int m_hitCount;
        public int HitCount => m_hitCount;
        [SerializeField] [ReadOnly] bool m_result;
        public bool Result => m_result;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        public RaycastHit2D[] Hits => hits;
        Collider2D[] colliders = new Collider2D[1];
        public Collider2D[] Colliders => colliders;

        [Header("Debug")]
        [SerializeField] CastType m_castType;
        public Color DebugColor;

        

        public bool SingleCast()
        {
            m_castType = CastType.RAY;

            m_hitCount = Physics2D.RaycastNonAlloc(transform.position, Direction, hits, Distance, Mask);
            m_result = m_hitCount > 0;
            return m_result;
        }

        public bool SingleCast(Vector3 position)
        {
            m_castType = CastType.RAY;

            m_hitCount = Physics2D.RaycastNonAlloc(position, Direction, hits, Distance, Mask);
            m_result = m_hitCount > 0;
            return m_result;
        }

        public bool OverlapCircle()
        {
            m_castType = CastType.CIRCLE;

            m_hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, Distance, colliders, Mask);
            m_result = m_hitCount > 0;
            return m_result;
        }

        public bool OverlapCircle(Vector3 position)
        {
            m_castType = CastType.CIRCLE;

            m_hitCount = Physics2D.OverlapCircleNonAlloc(position, Distance, colliders, Mask);
            m_result = m_hitCount > 0;
            return m_result;
        }

        public bool CircleCast(float distance)
        {
            m_hitCount = Physics2D.CircleCastNonAlloc(transform.position, Distance, Direction, hits, distance, Mask);
            m_result = m_hitCount > 0;
            return m_result;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        void Update()
        {
            DrawDebug();
        }
#endif


        public void DrawDebug()
        {
            switch (m_castType)
            {
                case CastType.RAY:
                    UnityEngine.Debug.DrawLine(transform.position, transform.position + (Distance * Direction), DebugColor);
                    break;
                case CastType.CIRCLE:
                    UnityEngine.Debug.DrawLine(transform.position, transform.position + (Distance * Direction), DebugColor);
                    break;
            }
            
            
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = DebugColor;
            switch (m_castType)
            {
                case CastType.RAY:
                    Gizmos.DrawLine(transform.position, transform.position + (Distance * Direction));
                    break;
                case CastType.CIRCLE:
                    Gizmos.DrawWireSphere(transform.position, Distance);
                    break;
            }
        }
    }
}