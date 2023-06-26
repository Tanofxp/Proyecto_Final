using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public string DriveType;

    public Car(string driveType)
    {
        DriveType = driveType;
    }

    private Rigidbody carRB;

    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;

    public float accInput;
    public float dirInput;
    public float frenoInput;

    public float MotorHP { get; set; }
    public float FrenoP { get; set; }
    public float SlipAngle { get; private set; }
    public float Speed { get; private set; }
    public AnimationCurve DireccionCurva;


    void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Speed = carRB.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplyDireccion();
        ApplyFreno();
        ApplyWheelPosition();
    }

    void CheckInput()
    {
        accInput = Input.GetAxis("Vertical");
        dirInput = Input.GetAxis("Horizontal");
        SlipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, carRB.velocity);
        frenoInput = 0f;

        if (movingDirection < -0.5f && accInput > 0)
        {
            frenoInput = Mathf.Abs(accInput);
        }
        else if (movingDirection > 0.5f && accInput < 0)
        {
            frenoInput = Mathf.Abs(accInput);
        }

        ApplyInputValues(accInput, dirInput, frenoInput);
    }

    void ApplyInputValues(float accInput, float dirInput, float frenoInput)
    {
        this.accInput = accInput;
        this.dirInput = dirInput;
        this.frenoInput = frenoInput;
    }

    void ApplyMotor()
    {
        switch (DriveType)
        {
            case "FWD":
                colliders.FRWheel.motorTorque = MotorHP * accInput;
                colliders.FLWheel.motorTorque = MotorHP * accInput;
                break;

            case "RWD":
                colliders.RRWheel.motorTorque = MotorHP * accInput;
                colliders.RLWheel.motorTorque = MotorHP * accInput;
                break;

            case "AWD":
                colliders.FRWheel.motorTorque = MotorHP * accInput;
                colliders.FLWheel.motorTorque = MotorHP * accInput;
                colliders.RRWheel.motorTorque = MotorHP * accInput;
                colliders.RLWheel.motorTorque = MotorHP * accInput;
                break;

            default:
                Debug.LogError("Unknown DriveType: " + DriveType);
                break;
        }
    }

    void ApplyDireccion()
    {
        float steeringAngle = dirInput * DireccionCurva.Evaluate(Speed);

        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyFreno()
    {
        colliders.FRWheel.brakeTorque = frenoInput * FrenoP * 0.9f;
        colliders.FLWheel.brakeTorque = frenoInput * FrenoP * 0.9f;
        colliders.RRWheel.brakeTorque = frenoInput * FrenoP * 0.5f;
        colliders.RLWheel.brakeTorque = frenoInput * FrenoP * 0.5f;
    }

    void ApplyWheelPosition()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;

        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FRWheel;
        public WheelCollider FLWheel;
        public WheelCollider RLWheel;
        public WheelCollider RRWheel;
    }

    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer FRWheel;
        public MeshRenderer FLWheel;
        public MeshRenderer RLWheel;
        public MeshRenderer RRWheel;
    }
}