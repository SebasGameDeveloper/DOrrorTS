using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct EnemyMovementData : IComponentData
    {
        public float speed;
        public float3 targetPosition;
        public bool isActive;
        public bool isFrozen;
    }
    
    public struct DetectionData : IComponentData
    {
        public float detectionRadius;
        public Entity playerEntity;
        public bool playerDetected;
    }
    
    public struct PursuitData : IComponentData
    {
        public float pursuitSpeed;
        public float3 lastKnownPlayerPosition;
        public bool isInPursuit;
    }
    
    public struct AudioData : IComponentData
    {
        public Entity audioSourceEntity;
        public float baseVolume;
        public float currentVolume;
        public float maxDistance;
    }
}