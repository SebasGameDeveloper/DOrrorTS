using Jobs;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(CameraSyncSystem))]
    public partial struct FreezeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cameraData = SystemAPI.GetSingleton<Components.MainCameraData>();

            var job = new FreezeJob
            {
                CameraPos = cameraData.Position,
                CameraForward = cameraData.Forward,
            };
            job.ScheduleParallel();
        }
    }
}