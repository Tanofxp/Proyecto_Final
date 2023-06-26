using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCar
{

    public class Steering : MonoBehaviour
    {


        [SerializeField] private float steerLock = 30.0f;

        [SerializeField] private float steerReturnLow = 40.0f;

        [SerializeField] private float steerReturnHigh = 20.0f;

        [SerializeField] private float steerAdjustLow = 40.0f;

        [SerializeField] private float steerAdjustHigh = 40.0f;

        [SerializeField] private float highVel = 15.0f;

        [SerializeField] private bool steerAssist = true;

        [SerializeField] private float maxSlip = 4.5f;


        private float timeStep;
        private float maxSlipRad;
        private WheelCollider[] wC;

        void Start()
        {
            wC = gameObject.GetComponentsInChildren<WheelCollider>();
            timeStep = (1.0f / Time.deltaTime);
            maxSlipRad = maxSlip * Mathf.Deg2Rad;
        }

        public float SteerAngle(float vel, float controllerInputX, float steerAngle)
        {
            float velocityScalar = 1.0f - Mathf.Clamp(Mathf.Abs(vel / highVel), 0.0f, 1.0f);
            float steerAdjustTotal = (steerAdjustHigh + (steerAdjustLow - steerAdjustHigh) * velocityScalar) / timeStep;
            float steerReturnTotal = (steerReturnHigh + (steerReturnLow - steerReturnHigh) * velocityScalar) / timeStep;

            float updatedSteerAngle;
            float slipLat = 0.0f;
            bool noMoreSteer = false;

            if (Mathf.Abs(controllerInputX) < 0.03f)
            {

                if (Mathf.Abs(steerAngle) > steerReturnTotal)
                {
                    updatedSteerAngle = steerAngle - steerReturnTotal * Mathf.Sign(steerAngle);
                }
                else updatedSteerAngle = 0.0f;
            }
            else
            {

                for (int i = 2; i < 4; i++)
                {
                    wC[i].GetGroundHit(out WheelHit contactPatch);
                    slipLat = contactPatch.sidewaysSlip;
                    if (Mathf.Abs(slipLat) > maxSlipRad) noMoreSteer = true;
                }


                updatedSteerAngle = steerAngle + steerAdjustTotal * Mathf.Sign(controllerInputX);

                if (Mathf.Abs(updatedSteerAngle) > steerLock) updatedSteerAngle = steerLock * Mathf.Sign(controllerInputX);

                if (noMoreSteer && steerAssist && (Mathf.Sign(slipLat) * Mathf.Sign(controllerInputX) < 0)) updatedSteerAngle = steerAngle;
            }

            return updatedSteerAngle;
        }



        public float AckerAdjusted(float steerAngle, float wheelBase, float trackFront, bool left)
        {
            if (steerAngle == 0.0f) return 0.0f;
            float turnRad = wheelBase / Mathf.Tan(Mathf.Deg2Rad * steerAngle);
            if (left) return Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRad + trackFront));
            else return Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRad - trackFront));
        }

    }
}