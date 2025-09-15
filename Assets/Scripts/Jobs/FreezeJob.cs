using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
    [BurstCompile]
    public partial struct FreezeJob : IJobEntity
    {
        public float3 CameraPos;
        public float3 CameraForward;

        [BurstCompile]
        public void Execute(ref Components.FreezeState freeze, in LocalTransform transform)
        {
            float3 toEnemy = transform.Position - CameraPos;
            float dot = math.dot(math.normalize(toEnemy), CameraForward); // Dot se usa para calcular la similitud entre dos vectores, determinar si un objeto está delante o detrás de otro
            if (dot > 0)
            {
                //Raycast
                freeze.IsFrozen = true; //Placeholder para probar el job
                freeze.FrozenAnimTime = 0f;
            }
            else freeze.IsFrozen = false;
        }
    }
}