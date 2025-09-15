using Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct AudioSyncSystem : ISystem
    {
        private NativeQueue<float> volumeQueue; //Main thread

        public void OnCreate(ref SystemState state)
        {
            volumeQueue = new NativeQueue<float>(Allocator.Persistent);
        }

        public void OnDestroy(ref SystemState state)
        {
            volumeQueue.Dispose();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerPos = SystemAPI.GetSingleton<LocalTransform>().Position;
            var job = new AudioVolumeJob
            {
                PlayerPos = playerPos,
                VolumeQueue = volumeQueue.AsParallelWriter()
            };
            job.ScheduleParallel();
        }
    }//Pendiente un while para mixer global
}