using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMover : MonoBehaviour, IEnemyMover
    {
        [Header("NavMesh")]
        public float stoppingDistance = 1.25f;

        private NavMeshAgent agent;
        private Transform self;
        private Transform target;

        public bool isMoving => agent &&  !agent .isStopped && agent.velocity.sqrMagnitude > 0.1f;
        public float currentSpeed => agent ? agent.velocity.magnitude : 0f;

        public void Initialize(Transform self, Transform target)
        {
            this.self = self;
            this.target = target;
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = true;
            agent.updatePosition = true;
        }

        public void Follow()
        {
            if (!agent || !target) return;
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        public void Stop()
        {
            if (!agent) return;
            agent.isStopped = true;
            agent.ResetPath();
        }

        public void SetEnabled(bool enabled)
        {
            if (agent) agent.enabled = enabled;
        }
    }
}