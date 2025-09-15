using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
    [BurstCompile]
    public  partial struct DetectionJob : IJobEntity
    {
        public float3 PlayerPos;
        public void Execute(in LocalTransform transform, ref Components.Detection detection)
        {
            detection.IsDetected = math.distance(transform.Position, PlayerPos) <= detection.Range; //Importante el SIMD :)
        }
    }
}