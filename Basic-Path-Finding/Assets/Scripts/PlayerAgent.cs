using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    #region Exposed Instance Variables
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float distanceRequired = 1.5f;

    [SerializeField]
    private MeshRenderer groundRender;

    [SerializeField]
    private Material successMaterial;

    [SerializeField]
    private Material failureMaterial;

    [SerializeField]
    private Material defaultMaterial;
    #endregion


    #region Private Instance Variables
    private Rigidbody playerRigidbody;

    private Vector3 originalPosition;

    private Vector3 originalTargetPosition;
    #endregion


    public override void Initialize()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.localPosition; 
        originalTargetPosition = target.transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.LookAt(target.transform);
        target.transform.localPosition = originalTargetPosition;

        transform.localPosition = originalPosition;
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, Random.Range(-4, 4));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observation
        sensor.AddObservation(transform.localPosition);

        // 3 observation
        sensor.AddObservation(target.transform.localPosition);

        // 1 observation
        sensor.AddObservation(playerRigidbody.velocity.x);

        // 1 observation
        sensor.AddObservation(playerRigidbody.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var vectorForce = new Vector3();
        vectorForce.x = vectorAction[0];
        vectorForce.z = vectorAction[1];

        playerRigidbody.AddForce(vectorForce * speed);

        var distanceFromTarget = Vector3.Distance(transform.localPosition, target.transform.localPosition);

        // we are doing good
        if (distanceFromTarget <= distanceRequired)
        {
            SetReward(1.0f);
            EndEpisode();

            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
        }

        // we are not doing so good
        if(transform.localPosition.y < 0)
        {
            EndEpisode();

            // go back and punish the agent for their performance

            StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical"); // x
        actionsOut[1] = Input.GetAxis("Horizontal"); // z
    }

    private IEnumerator SwapGroundMaterial(Material material, float time)
    {
        groundRender.material = material;
        yield return new WaitForSeconds(time);
        groundRender.material = defaultMaterial;
    }

}
