using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] float levelLoadDelayTime = 1f;
        [SerializeField] AudioClip crashAudio;
        [SerializeField] AudioClip successAudio;
        [SerializeField] ParticleSystem successParticles;
        [SerializeField] ParticleSystem crashParticles;

        AudioSource audioSource;
        float initialVolume;

        bool isTransitioning;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            initialVolume = audioSource.volume;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isTransitioning)
            {
                switch (collision.gameObject.tag)
                {
                    case ("Friendly"):
                        break;
                    case ("Finish"):
                        SuccessHandler();
                        break;
                    case ("Obstacle"):
                        CrashHandler();
                        break;
                }
            }
        }

        void SuccessHandler()
        {
            isTransitioning = true;
            StopAllCoroutines();
            GetComponent<Movement>().enabled = false;
            transform.rotation = Quaternion.identity;
            
            audioSource.Stop();
            audioSource.volume = initialVolume;
            audioSource.PlayOneShot(successAudio, 1f);
            successParticles.Play();

            Invoke("LoadNextLevel", levelLoadDelayTime);
        }

        void CrashHandler()
        {
            isTransitioning = true;
            StopAllCoroutines();
            GetComponent<Movement>().enabled = false;
            GetComponent<Movement>().enabled = false;
            DisableMeshRendererOnChildren();
            DisableLightsOnChildren();

            audioSource.Stop();
            audioSource.volume = initialVolume;
            audioSource.loop = false;
            audioSource.PlayOneShot(crashAudio, 1f);
            crashParticles.Play();
            Invoke("ReloadScene", levelLoadDelayTime);
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void LoadNextLevel()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }

        void DisableMeshRendererOnChildren()
        {
            foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = false;
            }
        }

        void DisableLightsOnChildren()
        {
            foreach (var light in GetComponentsInChildren<Light>())
            {
                light.enabled = false;
            }
        }
    }
}