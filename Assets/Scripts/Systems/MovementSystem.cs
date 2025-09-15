using Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

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

            var job = new PursuitJob
            {
                DeltaTime = deltaTime,
                PlayerPos = playerPos
            };
            job.ScheduleParallel();
        }
    }
}