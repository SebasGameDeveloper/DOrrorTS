using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
    [BurstCompile]
    public partial struct AudioVolumeJob : IJobEntity
    {
        public float3 PlayerPos;
        public NativeQueue<float>.ParallelWriter VolumeQueue;

        [BurstCompile]
        public void Execute(ref Components.AudioParams audio, in LocalTransform transform)
        {
            audio.DistanteToPlayer = math.distance(transform.Position, PlayerPos);
            audio.Volume = math.clamp(1f / (audio.DistanteToPlayer + 1f), 0.1f, 1f);
            VolumeQueue.Enqueue(audio.Volume);
        }
    }
}