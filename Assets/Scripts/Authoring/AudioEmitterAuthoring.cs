using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class AudioEmitterAuthoring : MonoBehaviour
    {
        class Baker : Baker<AudioEmitterAuthoring>
        {
            public override void Bake(AudioEmitterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Components.AudioParams());
                AddComponentObject(entity, authoring.GetComponent<AudioSource>());
            }
        }
    }
}