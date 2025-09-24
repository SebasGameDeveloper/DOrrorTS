using UnityEngine;

namespace Enemies
{
    public class EnemyAnimationController : MonoBehaviour, IEnemyAnimationController
    {
        [Header("Animator")]
        [SerializeField] private string walkStateName = "Walk";
        [SerializeField] private string speedParam = "Speed";

        private Animator anim;
        private int baseLayer = 0;
        private float cachedNormTime = 0f;

        public void Initialize(Animator animator) => anim = animator;

        public void PlayWalk()
        {
            if (!anim) return;
            AnimatorStateInfo st = anim.GetCurrentAnimatorStateInfo(baseLayer);
            if (st.IsName(walkStateName))
                anim.Play(walkStateName, baseLayer, 0f);

            anim.speed = 1f;
        }
        public void FreezeAtStart()
        {
            if (!anim) return;
            anim.Play(walkStateName, baseLayer, 0f);
            anim.Update(0f); 
            anim.speed = 0f;
        }
        

        public void FreezeAtCurrent()
        {
            if (!anim) return;
            var st = anim.GetCurrentAnimatorStateInfo(baseLayer);
            
            cachedNormTime = st.normalizedTime % 1f;
            anim.Play(walkStateName, baseLayer, cachedNormTime);
            anim.Update(0f);
            anim.speed = 0f;
        }

        public void Resume()
        {
            if (!anim) return;
            anim.Play(walkStateName, baseLayer, cachedNormTime);
            anim.speed = 1f;
        }

        public void SetLocomotionSpeed(float speedParamValue)
        {
            if (anim && !string.IsNullOrEmpty(speedParam))
                anim.SetFloat(speedParam, speedParamValue);
        }
    }
}