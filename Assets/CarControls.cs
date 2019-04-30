﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;

    public Transform LeftWheelVisual;
    public Transform RightWheelVisual;

    public bool Motor;
    public bool Steering;
}

public class CarControls : MonoBehaviour
{
    public List<AxleInfo> AxleInfos;
    public float MaxMotorTorque;
    public float BreakingTorque;
    public float MaxSteeringAngle;
    public float DownForce = 150;

    [HideInInspector] public float MotorInput;

    [HideInInspector] public float SteerInput;

    private Rigidbody componentRigidbody;

    public float CurrentSpeed { get; private set; }

    public bool IsGrounded { get; private set; }

    private void Start()
    {
        componentRigidbody = GetComponent<Rigidbody>();
        componentRigidbody.centerOfMass += Vector3.down * .5f;
    }

    public void FixedUpdate()
    {
        float torque = MotorInput * MaxMotorTorque;
        float brakes = MotorInput < 0 ? -MotorInput * BreakingTorque : 0;
        float steering = MaxSteeringAngle * SteerInput;

        foreach (AxleInfo axleInfo in AxleInfos)
        {
            axleInfo.LeftWheel.ConfigureVehicleSubsteps(5, 5, 1);

            if (axleInfo.Steering)
            {
                axleInfo.LeftWheel.steerAngle = steering;
                axleInfo.RightWheel.steerAngle = steering;
            }

            if (axleInfo.Motor)
            {
                axleInfo.LeftWheel.motorTorque = torque;
                axleInfo.RightWheel.motorTorque = torque;

                axleInfo.LeftWheel.brakeTorque = brakes;
                axleInfo.RightWheel.brakeTorque = brakes;
            }

            ApplyLocalPositionToVisuals(axleInfo.LeftWheel, axleInfo.LeftWheelVisual);
            ApplyLocalPositionToVisuals(axleInfo.RightWheel, axleInfo.RightWheelVisual);
        }

        CurrentSpeed = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;

        IsGrounded = Physics.Raycast(transform.position + transform.up * .25f, -transform.up, .5f);

        componentRigidbody.AddForce(0, -DownForce, 0);
    }

    private static void ApplyLocalPositionToVisuals(WheelCollider collider, Transform visual)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visual.position = position;
        visual.rotation = rotation;
    }
}