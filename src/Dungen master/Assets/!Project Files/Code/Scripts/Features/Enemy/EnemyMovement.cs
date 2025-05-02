using Progress;
using Services.Progress;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour,
        ILevelProgressLoadSubscriber, ILevelProgressCollector
    {
        public Vector3 CurrentDestination => _agent.destination;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            LevelProgressSaveCollectorsProvider.Instance.Subscribe(this);
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            LevelProgressSaveCollectorsProvider.Instance.Unsubscribe(this);
            EventBus.Unsubscribe(this);
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

        public void OnProgressLoaded(LevelSaveData progress)
        {
            var enemy = progress.enemies.Find(x => x.guid == gameObject.name);
            if (enemy == null)
            {
                return;
            }

            var agentIsStopped = _agent.isStopped;
            _agent.isStopped = true;
            _agent.transform.position = enemy.position;
            _agent.transform.rotation = enemy.rotation;
            _agent.isStopped = agentIsStopped;
        }

        public void Collect(LevelSaveData target)
        {
            var enemy = target.enemies.Find(x => x.guid == gameObject.name);
            if (enemy == null)
            {
                enemy = new EnemyData
                {
                    guid = gameObject.name
                };
                target.enemies.Add(enemy);
            }

            enemy.position = transform.position;
            enemy.rotation = transform.rotation;
        }
    }
}