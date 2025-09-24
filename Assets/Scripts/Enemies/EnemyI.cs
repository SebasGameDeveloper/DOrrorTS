using UnityEngine;

namespace Enemies
{
    public interface IEnemyPerception
    {
        bool InRange { get; }
        bool IsSeen { get; }
        float DistanceToPlayer { get; }
        void Initialize(Transform player, Camera playerCamera);
        void Tick();
    }

    public interface IEnemyMover
    {
        bool isMoving { get; }
        void Initialize(Transform self, Transform target);
        void Follow();
        void Stop();
        void SetEnabled(bool enabled);
        float currentSpeed { get; }
    }

    public interface IEnemyAnimationController
    {
        void Initialize(Animator animator);
        void PlayWalk();
        void FreezeAtStart();
        void FreezeAtCurrent();
        void Resume();
        void SetLocomotionSpeed(float speedParam);
    }
    
    public interface IEnemyAudioController
    {
        void Initialize(Transform self, Transform player);
        void OnMoving(bool moving);
        void OnFreeze();
        void OnResume();
        void UpdateProximity(float distanceNormalized);
    }
}