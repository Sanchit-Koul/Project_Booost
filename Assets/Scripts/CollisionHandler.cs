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
            audioSource.PlayOneShot(crashAudio);
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
    }
}