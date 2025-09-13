using Unity.Entities;

namespace Components
{
    public struct AudioParams : IComponentData
    {
        public float DistanteToPlayer;
        public float Volume;
        public bool Occluded;
    }
}