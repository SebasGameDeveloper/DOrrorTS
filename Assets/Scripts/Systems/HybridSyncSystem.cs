using Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

namespace Systems
{
    [BurstCompile] 
    public partial struct HybridSyncSystem : ISystem
    {
        private EntityQuery enemyQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAllRW<LocalTransform>()
                .WithAllRW<Components.FreezeState>()
                .WithAllRW<Components.Moveable>()
                .WithAllRW<Components.EnemyStats>();
            enemyQuery = builder.Build(ref state); 
            builder.Dispose();  //Manual dispose para el TempAllocator :) 
        }
        
        public void OnUpdate(ref SystemState state)  
        {
            var transformArray = new TransformAccessArray(enemyQuery.CalculateEntityCount());
            var entities = enemyQuery.ToEntityArray(Allocator.TempJob);
            var em = state.EntityManager;  //Cache EntityManager para accesos repetidos /Puede variar

            // Sincronización de transforms
            for (int i = 0; i < entities.Length; i++)
            {
                var go = em.GetComponentObject<GameObject>(entities[i]); 
                transformArray.Add(go.transform);
            }
            
            var syncJob = new SyncJob
            {
                Entities = entities,  
                EntityManager = em  // Pasa EM si SyncJob necesita, pero idealmente usa ComponentLookup para unmanaged
            };
            state.Dependency = syncJob.Schedule(transformArray, state.Dependency);  
            state.Dependency.Complete(); 
            
            foreach (var entity in entities)
            {
                var freeze = em.GetComponentData<Components.FreezeState>(entity);  //Unmanaged
                var anim = em.GetComponentObject<Animator>(entity);  //Managed

                if (anim != null)  //Null check
                {
                    anim.speed = freeze.IsFrozen ? 0f : 1f;
                    anim.Play(0, -1, freeze.FrozenAnimTime); 
                }
            }

            transformArray.Dispose();
            entities.Dispose(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            // Limpieza si necesario
        }
    }
}