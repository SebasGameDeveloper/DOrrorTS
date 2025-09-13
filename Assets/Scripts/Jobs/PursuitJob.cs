using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
    //Es importante alcarar que si aun job le falta un componente jamas se va a ejecutras
    //pero esto no genera ningun error y puede ser dificil de detectar :)
    public partial struct PursuitJob : IJobEntity
    {
        public float DeltaTime;
        public float3 PlayerPos;

        [BurstCompile]
        public void Execute(ref LocalTransform transform, ref Components.Moveable moveable,
            in Components.Detection detection, in Components.FreezeState freeze,
            in Components.EnemyStats enemyState)
        {
            if (!detection.IsDetected || freeze.IsFrozen)
            {
                moveable.Velocity = float3.zero;
                return;
            }
            
            float3 direction = math.normalize(PlayerPos - transform.Position);
            //moveable.Velocity = direction * stats
        }
    }
}