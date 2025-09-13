using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

namespace Systems
{
    [BurstCompile]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var playerPos = SystemAPI.GetSingleton<LocalTransform>().Position;

            var job = new PursiotJob
            {
                DeltaTime = deltaTime,
                PlayerPos = playerPos
            };
            job.ScheduleParallel();
        }
    }
}