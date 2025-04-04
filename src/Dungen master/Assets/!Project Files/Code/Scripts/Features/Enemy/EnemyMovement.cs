using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        public event Action<Vector3> OnDestinationReached;

        public Vector3 CurrentDestination => _agent.destination;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (_agent.hasPath &&
                _agent.isStopped == false &&
                _agent.remainingDistance <= _agent.stoppingDistance)
            {
                OnDestinationReached?.Invoke(CurrentDestination);
            }
        }

        public void MoveTo(Vector3 targetPosition)
        {
            _agent.SetDestination(targetPosition);
        }

        public void StopMoving()
        {
            _agent.isStopped = true;
        }

        public void ResumeMoving()
        {
            _agent.isStopped = false;
        }
    }
}