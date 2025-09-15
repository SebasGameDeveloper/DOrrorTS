using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct PlayerInputData : IComponentData
    {
        public float2 MoveInput;
        public float2 LookInput;
        public bool InteractedPressed;
    }
}