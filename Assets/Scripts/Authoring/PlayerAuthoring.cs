using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Entities;

namespace Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float walkSpeed = 3.5f;
        public InputActionAsset inputActions;

        class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Components.Moveable {Speed = authoring.walkSpeed});
                AddComponent(entity, new Components.PlayerInputData());
                //Hybrid ya que la camara y el input no son compaltibles con ECS
                AddComponentObject(entity, authoring.GetComponent<Camera>());
                AddComponentObject(entity, authoring.GetComponent<CharacterController>());
            }
        }
    }
}