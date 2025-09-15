using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public partial class DoorInteractionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (SystemAPI.GetSingleton<Components.PlayerInputData>().InteractedPressed)
            {
                //Interaccion input o camera por ray?
            } 
        }
    }
}