using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SocialPlatforms;


namespace Jobs
{
    public struct SyncJob : IJobParallelForTransform
    {
        [ReadOnly] public NativeArray<Entity> Entities;
        [ReadOnly] public EntityManager EntityManager;

        public void Execute(int index, TransformAccess transform)
        {
            Entity entity = Entities[index];
            if (EntityManager.Exists(entity))
            {
                LocalTransform lt = EntityManager.GetComponentData<LocalTransform>(entity);
                transform.position = lt.Position;
                transform.rotation = lt.Rotation;
            }
        }
    }
}