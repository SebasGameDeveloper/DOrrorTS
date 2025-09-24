using UnityEngine;
using UnityEngine.Audio;

namespace Enemies
{
    public class EnemyAudioController : MonoBehaviour, IEnemyAudioController
    {
        [Header("Fuentes")]
        [SerializeField] private AudioSource footsteps;   //Loop suave y random pitch
        [SerializeField] private bool autoRandomizePitch = true;
        [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

        [Header("Mixer")]
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private string musicIntensityParam = "MusicIntensity";

        [Header("Distancias para tensión (m)")]
        [SerializeField] private float minTensionDistance = 2f;
        [SerializeField] private float maxTensionDistance = 20f;

        private Transform player;
        private Transform selfT;
            
        public void Initialize(Transform self, Transform player)
        {
            selfT = self;
            this.player = player;

            if (footsteps)
            {
                footsteps.loop = true;
                footsteps.spatialBlend = 1f;
                footsteps.rolloffMode = AudioRolloffMode.Linear;
                footsteps.minDistance = 1.5f;
                footsteps.maxDistance = 20f;
                footsteps.playOnAwake = false;
            }
        }

        public void OnMoving(bool moving)
        {
            if (!footsteps) return;

            if (moving)
            {
                if (!footsteps.isPlaying)
                {
                    if (autoRandomizePitch)
                        footsteps.pitch = Random.Range(pitchRange.x, pitchRange.y);
                    footsteps.Play();
                }
            }
            else
            {
                if (footsteps.isPlaying) footsteps.Pause();
            }
        }

        public void OnFreeze()
        {
            if (footsteps && footsteps.isPlaying) footsteps.Pause();
        }

        public void OnResume()
        {
            if (footsteps && !footsteps.isPlaying) footsteps.UnPause();
        }

        public void UpdateProximity(float distanceNormalized)
        {
            if (!mixer || !player || !selfT) return;

            float d = Vector3.Distance(player.position, selfT.position);
            float t = Mathf.InverseLerp(maxTensionDistance, minTensionDistance, d);
            mixer.SetFloat(musicIntensityParam, Mathf.Clamp01(t));
        }
    }
}