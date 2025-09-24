using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyPerception : MonoBehaviour, IEnemyPerception
    {
        [Header("Dependencias")] 
        [SerializeField] private Transform aimPoint;
        [SerializeField] private LayerMask visionMask;

        [Header("Rango (m)")] 
        [SerializeField] private float enterRange = 20f;
        [SerializeField] private float exitRange = 22f;

        [Header("Campo de visión (grados)")] 
        [Tooltip("1 = estrictamente el rayo al centro.")] [Range(0f, 30f)]
        public float angleAllowanceDeg = 5f;
        
        [FormerlySerializedAs("drwaDebug")] [Header("Debug")] 
        public bool drawDebug = false;
        
        private Transform player;
        private Camera playerCam;

        public bool InRange { get; private set; }
        public bool IsSeen { get; private set; }
        public float DistanceToPlayer { get; private set; }

        public void Initialize(Transform player, Camera playerCamera)
        {
            this.player = player;
            this.playerCam = playerCamera;
        }
        
        public void Tick()
        {
            if(!player || !playerCam || !aimPoint) return;
            
            //Histérisis de rango :)
            DistanceToPlayer = Vector3.Distance(player.position, aimPoint.position);
            if (!InRange && DistanceToPlayer <= enterRange) InRange = true;
            else if (InRange && DistanceToPlayer <= exitRange) InRange = false;
            
            if (!InRange){ IsSeen = false; return; }
            
            Vector3 camPos = playerCam.transform.position;
            Vector3 toEnemy = (aimPoint.position - camPos);
            Vector3 camFwd = playerCam.transform.forward;
            
            //enemigo en el centro del campo de visión? Verificacion en playtest OJO!
            float angle = Vector3.Angle(camFwd, toEnemy);
            if (angle > angleAllowanceDeg) { IsSeen = false; return; }
            
            //Ray 
            float dist = toEnemy.magnitude;
            if (Physics.Raycast(camPos, toEnemy.normalized, out RaycastHit hit, dist + 0.05f, visionMask, QueryTriggerInteraction.Ignore))
            {
                IsSeen = hit.transform == aimPoint || hit.transform.IsChildOf(aimPoint);
            }
            else
            {
                IsSeen = false;
            }

            if (drawDebug)
            {
                Debug.DrawRay(camPos, toEnemy.normalized * dist, IsSeen ? Color.green : Color.red );
            }

        }
    }
}