using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class DoorAuthoring : MonoBehaviour
    {
        class Baker : Baker<DoorAuthoring>
        {
            public override void Bake(DoorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, authoring.GetComponent<Animator>());
            }
        }
    }
}