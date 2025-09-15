using Unity.Entities;

namespace Components
{
    public struct Detection :IComponentData
    {
        public float Range;
        public bool IsDetected;
    }
}