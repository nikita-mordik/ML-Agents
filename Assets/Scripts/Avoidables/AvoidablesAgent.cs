using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace FreedLOW.MLAgents.Avoidables
{
    public class AvoidablesAgent : Agent
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 3f;
        [SerializeField] private float spawnRadiusWhenReset = 5f;
        [SerializeField] private float rewardPerStep = 0.05f;
        [SerializeField] private float centerDistanceReward = 0.05f;
        [SerializeField] private float maxDistanceForCenterReward = 5f;

        [SerializeField] private UnityEvent OnEpisodeBegin;

        private float _distanceToCenter;
        private float _distanceToCenterPercentage;

        private void FixedUpdate()
        {
            body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);
            var localPosition = transform.localPosition;
            localPosition.y = 0.25f;
            transform.localPosition = localPosition;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<AvoidableObstacleController>())
            {
                FailAgent();
            }
        }

        public override void Initialize()
        {
            var spawnPosition = GetSpawnPosition();
            transform.localPosition = spawnPosition;
            var maxReward = MaxStep * (rewardPerStep + centerDistanceReward);
            Debug.Log($"Agent initialize complete. Theoretical max reward: {maxReward}");
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;

            // Increase body velocity:
            continuousActionsOut[0] = Input.GetAxis("Horizontal");
            continuousActionsOut[1] = Input.GetAxis("Vertical");
        }

        public override void OnActionReceived(ActionBuffers actionsBuffer)
        {
            var inputX = Mathf.Clamp(actionsBuffer.ContinuousActions[0], -1f, 1f);
            var inputZ = Mathf.Clamp(actionsBuffer.ContinuousActions[1], -1f, 1f);

            Vector3 force = new Vector3(inputX, 0f, inputZ);
            body.AddForce(force * acceleration, ForceMode.Force);
            AddReward(rewardPerStep);
            AddReward(centerDistanceReward * _distanceToCenterPercentage);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(body.velocity);

            _distanceToCenter = Vector3.Distance(transform.localPosition, Vector3.zero);
            _distanceToCenterPercentage = _distanceToCenter / maxDistanceForCenterReward;
            _distanceToCenterPercentage = Mathf.Clamp01(_distanceToCenterPercentage);
            _distanceToCenterPercentage = 1f - _distanceToCenter;
            
            sensor.AddObservation(_distanceToCenter);
            sensor.AddObservation(_distanceToCenterPercentage);
        }

        private void FailAgent()
        {
            AddReward(-1f);
            OnEpisodeBegin?.Invoke();
            
            var spawnPosition = GetSpawnPosition();
            body.position = transform.parent.TransformPoint(spawnPosition);
            body.velocity = Vector3.zero;
        }

        private Vector3 GetSpawnPosition()
        {
            var spawnPosition = Random.insideUnitSphere * spawnRadiusWhenReset;
            spawnPosition.y = transform.parent.position.y + transform.localScale.y / 2f;
            return spawnPosition;
        }
    }
}