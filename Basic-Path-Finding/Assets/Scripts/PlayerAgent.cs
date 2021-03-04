using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    [SerializeField]
    private float speed = 10.0f;

    private Rigidbody playerRigidbody;

    [SerializeField]
    private GameObject target;

    private Vector3 originalPosition;

    public override void Initialize()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.transform.localPosition);

        sensor.AddObservation(playerRigidbody.velocity.x);
        sensor.AddObservation(playerRigidbody.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var vectorForce = new Vector3();
        vectorForce.x = vectorAction[0];
        vectorForce.z = vectorAction[1];

        playerRigidbody.AddForce(vectorForce * speed);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal"); // x
        actionsOut[1] = Input.GetAxis("Vertical"); // z
    }
}
