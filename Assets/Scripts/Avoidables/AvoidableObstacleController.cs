using UnityEngine;
using Random = UnityEngine.Random;

namespace FreedLOW.MLAgents.Avoidables
{
    public class AvoidableObstacleController : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float speed = 2f;

        private Vector3 _direction;

        private void OnEnable()
        {
            _direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            body.velocity = _direction * speed;
        }

        private void FixedUpdate()
        {
            body.velocity = _direction * speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            _direction = Vector3.Reflect(_direction, other.contacts[0].normal);
            _direction.y = 0f;
            body.velocity = _direction * speed;
        }
    }
}