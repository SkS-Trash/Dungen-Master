using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        public Vector3 CurrentDestination => _agent.destination;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 targetPosition)
        {
            _agent.SetDestination(targetPosition);
            _agent.isStopped = false;
        }

        public void StopMoving()
        {
            _agent.isStopped = true;
        }

        public void ResumeMoving()
        {
            _agent.isStopped = false;
        }

        public bool HasReachedDestination() =>
            !_agent.pathPending &&
            _agent.remainingDistance < 0.5f;

        public void SetSpeed(float chaseSpeed) =>
            _agent.speed = chaseSpeed;

        public void SetStoppingDistance(float stoppingDistance, bool autoBraking)
        {
            _agent.stoppingDistance = stoppingDistance;
            _agent.autoBraking = autoBraking;
        }
    }
}