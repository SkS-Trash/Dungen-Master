using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode] public Vector3 CurrentDestination => _agent.destination;

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
            _agent.hasPath &&
            _agent.isStopped == false &&
            _agent.remainingDistance <= _agent.stoppingDistance;
    }
}