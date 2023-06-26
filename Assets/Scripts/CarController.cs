using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UnityCar
{

    public class CarController : MonoBehaviour
    {
        [SerializeField] private float mass = 1200.0f;
        [SerializeField] private Vector3 coG = new Vector3(0.0f, 0.435f, -2.5f);
        [SerializeField] private Vector3 inertiaTensor = new Vector3(3600.0f, 3900.0f, 800.0f);
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        

        public float GetVel { get { return vel; } }
        public float GetMass { get { return mass; } }
        public Vector3 GetCoG { get { return coG; } }
        public Rigidbody GetRB { get { return rB; } }

        private WheelCollider[] wC;
        private Rigidbody rB;
        private float vel;
        private float steerAngle = 0.0f;
        private float carSpeed;

        private AeroDynamics aeroDynamics;
        private Brakes brakes;
        private Engine engine;
        private Steering steering;
        private Suspension suspension;
        private Transmission transmission;
        private UserInput userInput;

        void Awake()
        {
            aeroDynamics = GetComponent<AeroDynamics>();
            brakes = GetComponent<Brakes>();
            engine = GetComponent<Engine>();
            steering = GetComponent<Steering>();
            suspension = GetComponent<Suspension>();
            transmission = GetComponent<Transmission>();
            userInput = GetComponent<UserInput>();


            Time.fixedDeltaTime = 0.008333f;

            wC = gameObject.GetComponentsInChildren<WheelCollider>();

            rB = GetComponent<Rigidbody>();
            rB.mass = mass;
            rB.centerOfMass = coG;
            rB.inertiaTensor = inertiaTensor;
            rB.isKinematic = false;
        }


        void FixedUpdate()
        {

            float inputX = 0.0f;
            float inputY = 0.0f;
            float inputR = 0.0f;
            float inputH = 0.0f;

            if (userInput.enabled)
            {
                inputX = userInput.ControllerInputX;
                inputY = userInput.ControllerInputY;
                inputR = userInput.ControllerInputReverse;
                inputH = userInput.ControllerInputHandBrake;
            }

            vel = transform.InverseTransformDirection(rB.velocity).z;


            aeroDynamics.ApplyAeroDrag(vel);
            aeroDynamics.ApplyAeroLift(vel);

            steerAngle = steering.SteerAngle(vel, inputX, steerAngle);
            wC[2].steerAngle = steering.AckerAdjusted(steerAngle, suspension.GetWheelBase, suspension.GetTrackFront, true);
            wC[3].steerAngle = steering.AckerAdjusted(steerAngle, suspension.GetWheelBase, suspension.GetTrackFront, false);


            suspension.ApplyLLT();

            if (transmission.GetAutomatic)
            {
                if (inputR > 0.1) transmission.SelectReverse();
                else transmission.SetGear(suspension.GetNoSlipWheelRPM(vel), engine.GetEngineRPMMaxPower);
            }

            float transmissionRatio = transmission.GetTransmissionRatio();
            float engineClutchLockRPM = transmission.GetEngineClutchLockRPM;
            engine.UpdateEngineSpeedRPM(suspension.GetNoSlipWheelRPM(vel), inputY, transmissionRatio, engineClutchLockRPM);
            float engineTorque = engine.GetMaxEngineTorque();

            if (inputY > 0.2f) engineTorque *= inputY;
            else engineTorque = 0.0f;

            float[] wheelTorques = transmission.GetWheelTorques(engineTorque);

            float[] brakeTorques = brakes.GetBrakeTorques(inputY);

            if (inputH > 0.1f) brakes.ApplyHandbrake(brakeTorques);

            float wheelRPMLimit = Mathf.Abs(engine.GetEngineRPMMax / transmission.GetTransmissionRatio()) * 1.01f;

            int iDrivenWheelID;
            for (int j = 0; j < transmission.GetDrivenWheels.Count; j++)
            {
                iDrivenWheelID = transmission.GetDrivenWheels[j];
                if (wC[transmission.GetDrivenWheels[j]].rpm > wheelRPMLimit) wheelTorques[iDrivenWheelID] = 0.0f;
            }

            for (int i = 0; i < 4; i++)
            {
                wC[i].brakeTorque = brakeTorques[i];
                wC[i].motorTorque = wheelTorques[i];
            }

        }


        void Update()
        {
            Transform transWheel;

            carSpeed = Mathf.RoundToInt(GetRB.velocity.magnitude * 3600 / 1000);

            textMeshProUGUI.SetText(carSpeed.ToString() + "Km/h");

            if (!transmission.GetAutomatic)
            {
                if (Input.GetButtonDown("GearShiftUp")) transmission.GearShiftUp();
                if (Input.GetButtonDown("GearShiftDown")) transmission.GearShiftDown();
            }


            for (int i = 0; i < 4; i++)
            {
                wC[i].GetWorldPose(out Vector3 wcPosition, out Quaternion wcRotation);
                transWheel = wC[i].gameObject.transform.GetChild(0);
                transWheel.transform.position = wcPosition;
                transWheel.transform.localPosition = new Vector3(transWheel.transform.localPosition.x, transWheel.transform.localPosition.y, transWheel.transform.localPosition.z);
                transWheel.transform.rotation = wcRotation;
            }
        }

    }
}