using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class GameStateSystem : SystemBase
    {
        private float timer = 30f;
        private bool gameOver;

        protected override void OnUpdate()
        {
            timer -= SystemAPI.Time.DeltaTime;
            if (timer <= 0 && !gameOver)
            {
                // Win: Show UI, reset via ECB (EntityCommandBuffer para set positions)
            }

            Entities.ForEach((in Components.Moveable move, in LocalTransform trans) => {
                // Check attack dist para lose
            }).Run(); // Run: UI triggers y Audio.Play no thread-safe.
        }
    }
}