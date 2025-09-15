using Jobs;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    public partial struct FreezeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cameraPos = Camera.main.transform.position;
            var cameraForward = Camera.main.transform.forward;

            var job = new FreezeJob
            {
                CameraPos = cameraPos,
                CameraForward = cameraForward
            };
            job.ScheduleParallel();
        }
    }
}