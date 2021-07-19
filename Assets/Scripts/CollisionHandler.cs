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
                        Debug.Log("Friendly");
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
            successParticles.Play();
            audioSource.Stop();
            audioSource.volume = initialVolume;
            audioSource.PlayOneShot(successAudio);
            Invoke("LoadNextLevel", levelLoadDelayTime);
        }

        void CrashHandler()
        {
            isTransitioning = true;
            StopAllCoroutines();
            GetComponent<Movement>().enabled = false;
            audioSource.Stop();
            audioSource.volume = initialVolume;
            Debug.Log("Starting volume for crash : " + audioSource.volume);
            DisableMeshRendererOnChildren();
            audioSource.PlayOneShot(crashAudio, 1f);
            crashParticles.Play();
            //GetComponent<Rigidbody>().gameObject.SetActive(false);
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
    }
}