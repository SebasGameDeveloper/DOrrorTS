using UnityEngine;

namespace Enemies
{
    [DisallowMultipleComponent]
    public class EnemyController : MonoBehaviour
    {
        public enum EnemyState
        {
            IdleFrozen,
            Walk,
            SeenFrozen
        }

        [Header("Refs")] [SerializeField] private Animator animator;
        [SerializeField] private EnemyPerception perception;
        [SerializeField] private NavMeshMover mover;
        [SerializeField] private EnemyAnimationController animCtrl;
        [SerializeField] private EnemyAudioController audioCtrl;

        [Header("Targets")] [SerializeField] private Transform player;
        [SerializeField] private Camera playerCamera;

        [Header("Opciones")] [SerializeField] private bool enableGizmos = true;

        public EnemyState State { get; private set; } = EnemyState.IdleFrozen;

        private void Awake()
        {
            if (!animCtrl) animCtrl = GetComponent<EnemyAnimationController>();
            if (!perception) perception = GetComponent<EnemyPerception>();
            if (!mover) mover = GetComponent<NavMeshMover>();
            if (!audioCtrl) audioCtrl = GetComponent<EnemyAudioController>();
        }

        private void Start()
        {
            //Búsqueda básica
            if (!player) player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (!playerCamera) playerCamera = Camera.main;

            perception.Initialize(player, playerCamera);
            mover.Initialize(transform, player);
            animCtrl.Initialize(animator);
            audioCtrl.Initialize(transform, player);

            //Arranca congelado
            animCtrl.FreezeAtStart();
            mover.Stop();
            audioCtrl.OnFreeze();
        }

        private void Update()
        {
            perception.Tick();
            TickStateMachine();
            //actualizar audio de tensión
            audioCtrl.UpdateProximity(0f); //el método calcula internamente por distancia real
            //sincronizar parámetro de animación si lo usas
            animCtrl.SetLocomotionSpeed(mover.currentSpeed);
        }

        private void TickStateMachine()
        {
            switch (State)
            {
                case EnemyState.IdleFrozen:
                    if (perception.InRange && !perception.IsSeen)
                        GoTo(EnemyState.Walk);
                    if (perception.IsSeen)
                        GoTo(EnemyState.SeenFrozen);
                    break;

                case EnemyState.Walk:
                    if (!perception.InRange)
                        GoTo(EnemyState.IdleFrozen);
                    else if (perception.IsSeen)
                        GoTo(EnemyState.SeenFrozen);
                    else
                        mover.Follow();
                    break;

                case EnemyState.SeenFrozen:
                    if (!perception.IsSeen)
                    {
                        //si deja de verlo, y está en rango → caminar
                        if (perception.InRange) GoTo(EnemyState.Walk);
                        else GoTo(EnemyState.IdleFrozen);
                    }

                    break;
            }
        }

        private void GoTo(EnemyState next)
        {
            if (State == next) return;

            // Salidas simples
            switch (next)
            {
                case EnemyState.IdleFrozen:
                    mover.Stop();
                    animCtrl.FreezeAtStart();
                    audioCtrl.OnFreeze();
                    break;

                case EnemyState.Walk:
                    animCtrl.Resume(); //reanuda animación
                    animCtrl.PlayWalk(); //asegura estado
                    audioCtrl.OnResume();
                    mover.Follow();
                    break;

                case EnemyState.SeenFrozen:
                    mover.Stop();
                    animCtrl.FreezeAtCurrent();
                    audioCtrl.OnFreeze();
                    break;
            }

            State = next;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!enableGizmos || !perception) return;
            Gizmos.color = perception.InRange ? Color.green : Color.gray;
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        }
#endif
    }
}