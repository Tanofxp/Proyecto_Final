using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCar
{

    public class Tyres : MonoBehaviour
    {

        [SerializeField] private float tyreFrictionFront = 1.1f;
        [SerializeField] private float tyreFrictionRear = 1.1f;

        private readonly WheelFrictionCurve[] awfcLong = new WheelFrictionCurve[4];
        private readonly WheelFrictionCurve[] awfcLat = new WheelFrictionCurve[4];

        public WheelFrictionCurve[] GetWFCLong { get { return awfcLong; } }
        public WheelFrictionCurve[] GetWFCLat { get { return awfcLat; } }

        void Start()
        {

            WheelCollider[] wC = gameObject.GetComponentsInChildren<WheelCollider>();

            for (int i = 0; i < 2; i++)
            {

                awfcLong[i].extremumSlip = 0.15f;
                awfcLong[i].extremumValue = 1.0f;
                awfcLong[i].asymptoteSlip = 0.70f;
                awfcLong[i].asymptoteValue = 0.60f;
                awfcLong[i].stiffness = tyreFrictionRear;

                awfcLat[i].extremumSlip = 0.11f;
                awfcLat[i].extremumValue = 1.0f;
                awfcLat[i].asymptoteSlip = 0.80f;
                awfcLat[i].asymptoteValue = 0.70f;
                awfcLat[i].stiffness = tyreFrictionRear;
            }

            for (int i = 2; i < 4; i++)
            {

                awfcLong[i].extremumSlip = 0.15f;
                awfcLong[i].extremumValue = 1.0f;
                awfcLong[i].asymptoteSlip = 0.70f;
                awfcLong[i].asymptoteValue = 0.70f;
                awfcLong[i].stiffness = tyreFrictionFront;

                awfcLat[i].extremumSlip = 0.11f;
                awfcLat[i].extremumValue = 1.0f;
                awfcLat[i].asymptoteSlip = 0.80f;
                awfcLat[i].asymptoteValue = 0.70f;
                awfcLat[i].stiffness = tyreFrictionFront;
            }



            for (int i = 0; i < 4; i++)
            {
                wC[i].ConfigureVehicleSubsteps(30, 8, 20);
                wC[i].forwardFriction = awfcLong[i];
                wC[i].sidewaysFriction = awfcLat[i];
            }

        }

    }
}