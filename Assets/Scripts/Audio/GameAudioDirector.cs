using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class GameAudioDirector : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private string musicBusVolumeParam = "MusicVol"; // dB expuesto
        [SerializeField] private AnimationCurve proximityToMusicDb = AnimationCurve.Linear(0, 0, 1, -10);
        //Nota: si ya usas MusicIntensity (0..1), esta clase puede no ser necesaria. 
        //Aquí mostramos otra forma: mapear a dB.

        public void SetProximity01(float proximity01)
        {
            float db = proximityToMusicDb.Evaluate(Mathf.Clamp01(proximity01));
            mixer.SetFloat(musicBusVolumeParam, db);
        }
    }
}