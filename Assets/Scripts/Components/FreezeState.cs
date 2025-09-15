using Unity.Entities;
    
namespace Components
{
    public struct FreezeState : IComponentData
    {
        public bool IsFrozen;
        public float FrozenAnimTime;
    }
}