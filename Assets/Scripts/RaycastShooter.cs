using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityCar;
using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    [SerializeField] private Transform m_reycastPoint;
    [SerializeField] private float m_maxDistance;
    [SerializeField] private LayerMask m_raycastLayer;
    [SerializeField] private GameObject car;
    private CarController carController;

    void Awake()
    {
        carController = car.GetComponent<CarController>();
    }

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
                carController.GetRB.velocity *= 0.9999f;
            }
            if (l_hit.collider.name == "Asfalto")
            {
                carController.GetRB.velocity *= 1.0f;
            }
        }

    }
}

