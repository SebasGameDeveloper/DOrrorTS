using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct MainCameraData : IComponentData
    {
        public float3 Position;
        public float3 Forward;
    }
}