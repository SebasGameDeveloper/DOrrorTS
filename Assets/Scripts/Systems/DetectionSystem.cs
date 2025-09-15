using Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial struct DetectionSystem : ISystem
    {
        //private EntityQuery enemyQuery;

        /*public void OnCreate(ref SystemState state)
        {
            enemyQuery = state.GetEntityQuery
                (ComponentType.ReadOnly<LocalTransform>(), ComponentType.ReadWrite<Components.Detection>());
        }*/

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerPos = SystemAPI.GetSingleton<LocalTransform>().Position;
            //ar chunkCount = enemyQuery.CalculateChunkCount();
            //var distance = new NativeArray<float>(chunkCount * 128, Allocator.TempJob); //128 es el numero maximo de entidades por chunk :)

            var job = new Jobs.DetectionJob
            {
                PlayerPos = playerPos,
            };
            job.ScheduleParallel();
        }
    }
}