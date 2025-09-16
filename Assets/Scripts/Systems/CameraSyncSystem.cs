using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public partial class CameraSyncSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Camera.main == null) return;

            var camera = Camera.main.transform;
            var data = new Components.MainCameraData
            {
                Position = camera.position,
                Forward = camera.forward
            };

            if (!SystemAPI.HasSingleton<Components.MainCameraData>())
            {
                var entity = EntityManager.CreateEntity();
                EntityManager.AddComponent<Components.MainCameraData>(entity);
            }
            SystemAPI.SetSingleton(data);
        }
    }
}