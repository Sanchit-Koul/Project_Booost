using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public static class AudioSourceFadeEffect
    {
        public static IEnumerator FadeAudio(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
    }
}