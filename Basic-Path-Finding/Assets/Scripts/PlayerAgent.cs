using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    [SerializeField]
    private float speed = 10.0f;

    private Rigidbody playerRigidbod;

    [SerializeField]
    private GameObject target;

    public override void Initialize()
    {
        playerRigidbod = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        base.OnActionReceived(vectorAction);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal"); // x
        actionsOut[1] = Input.GetAxis("Vertical"); // z
    }
}
