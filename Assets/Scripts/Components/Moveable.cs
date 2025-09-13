using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Moveable :IComponentData
    {
        public float3 Position;
        public float3 Velocity;
        public float Speed;
    }
}