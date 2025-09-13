using Unity.Entities;

namespace Components
{
    public struct EnemyStats : IComponentData
    {
        public float PursuitSpeed;
        public float AttackDistance;
    }
}