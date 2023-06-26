using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RaycastShooter : MonoBehaviour
{
    [SerializeField] private Transform m_reycastPoint;
    [SerializeField] private float m_maxDistance;
    [SerializeField] private LayerMask m_raycastLayer;
    void Update()
    {

        DoRaycast();

    }
    private void DoRaycast()
    {
        bool l_isHitting = Physics.Raycast(m_reycastPoint.position, m_reycastPoint.forward, out RaycastHit l_hit, m_maxDistance, m_raycastLayer);

        if (l_isHitting)
        {
            if (l_hit.collider.name == "Tierra")
            {
                Debug.Log($"Estoy sobre Tierra -1 speed");
            }
            if (l_hit.collider.name == "Asfalto")
            {
                Debug.Log($"Estoy sobre Asfalto +1 speed");
            }
        }

    }
}

