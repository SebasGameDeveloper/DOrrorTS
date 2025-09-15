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
    [BurstCompile]  // Habilita Burst para partes compilables
    public partial struct HybridSyncSystem : ISystem
    {
        private EntityQuery enemyQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // Define query para enemigos con hybrid components
            enemyQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<LocalTransform>(),
                ComponentType.ReadOnly<Components.FreezeState>(),
                ComponentType.ReadOnly<Components.Moveable>(), 
                ComponentType.ReadOnly<Animator>() // Incluye ComponentObject para Animator
            );
        }

        [BurstCompile]  // Burst para la parte no-hybrid (el sync anim no lo es)
        public void OnUpdate(ref SystemState state)
        {
            // Crear TransformAccessArray para batch sync de Transforms (de GO)
            var transformArray = new TransformAccessArray(enemyQuery.CalculateEntityCount());

            // Fill transforms from GO: Iterar entidades y agregar Transform del GO ligado
            var entities = enemyQuery.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < entities.Length; i++)
            {
                var go = state.EntityManager.GetComponentObject<GameObject>(entities[i]);  // Asume added en Baker
                transformArray.Add(go.transform);
            }

            // Schedule job para sync positions (ECS -> GO)
            var syncJob = new SyncJob
            {
                Entities = entities,  // Compartido, no nuevo allocation
                EntityManager = state.EntityManager
            };
            state.Dependency = syncJob.Schedule(transformArray, state.Dependency);  // Chain dependency, no Complete() bloqueante

            // Post-job: Apply anim freeze from FreezeState (main thread only, sin Burst)
            state.Dependency.Complete();  // Completa todo antes de ForEach (si necesario para sync)

            SystemAPI.QueryBuilder()
                .WithAll<Components.EnemyStats, Animator>()  // Filtra solo enemigos
                .WithComponentObject<Animator>(true)  // Explícito para hybrid, resuelve analyzers
                .Build()
                .ForEach((in Components.FreezeState freeze, Animator anim) =>
                {
                    if (freeze.IsFrozen)
                    {
                        anim.speed = 0f;
                    }
                    else
                    {
                        anim.speed = 1f;
                    }
                    anim.Play(0, -1, freeze.FrozenAnimTime);  // Reproduce desde frame congelado
                })
                .WithoutBurst()  // Sin Burst: Animator no thread-safe
                .Run();  // Run: Main thread requerido para Animator

            // Cleanup (auto con Dependency, pero manual para TempJob)
            transformArray.Dispose(/*state.Dependency*/);
            entities.Dispose(state.Dependency);
        }

        public void OnDestroy(ref SystemState state)
        {
            // Cleanup
        }
    }
}