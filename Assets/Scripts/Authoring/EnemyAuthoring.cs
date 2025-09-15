using UnityEngine;
using Unity.Entities;
using UnityEngine.AI;

namespace Authoring
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float detectionRange = 10f;
        public float pursuitSpeed = 4f;
        public float attackDistance = 0.5f;
        public AudioClip footstepsClip;

        class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Components.Moveable {Speed = authoring.pursuitSpeed});
                AddComponent(entity, new Components.Detection { Range = authoring.detectionRange });
                AddComponent(entity, new Components.FreezeState());
                AddComponent(entity, new Components.AudioParams());
                AddComponent(entity, new Components.EnemyStats {PursuitSpeed = authoring.pursuitSpeed});
                
                
                AddComponentObject(entity, authoring.gameObject);
                //Hybrid para linkear GameObjects para Animator/Audio y NavMeshAgent
                AddComponentObject(entity, authoring.GetComponent<Animator>());
                AddComponentObject(entity, authoring.GetComponent<AudioSource>());
                AddComponentObject(entity, authoring.GetComponent<NavMeshAgent>());
            }
        }
    }
}