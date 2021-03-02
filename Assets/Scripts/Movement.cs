using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustForce = 0.0f;
    [SerializeField] float rotationThrust = 0.0f;
    [SerializeField] float rotationControlThrust = 0.0f;
    float startVolume = 0f;
    Rigidbody playerRb;
    AudioSource audioSource;
    Coroutine fadeOutCouroutine;
    Coroutine fadeInCouroutine;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        startVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform = GetComponent<Transform>();
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.W))
        {
            FadeInAudioEffect();
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            playerRb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.W) && audioSource.isPlaying)
            {
                FadeOutAudioEffect();
            }
        }
    }

    void FadeOutAudioEffect()
    {
        Debug.Log("Starting fade out coroutine");
        if (fadeInCouroutine != null)
        {
            StopCoroutine(fadeInCouroutine);
            fadeInCouroutine = null;
        }
        fadeOutCouroutine = StartCoroutine(AudioSourceFadeEffect.FadeAudio(audioSource, 1, 0));
    }

    void FadeInAudioEffect()
    {
        if (fadeOutCouroutine != null)
        {
            Debug.Log("Stopping fade out coroutine");
            StopCoroutine(fadeOutCouroutine);
            fadeOutCouroutine = null;
            Debug.Log("Starting fade in coroutine");
            fadeInCouroutine = StartCoroutine(AudioSourceFadeEffect.FadeAudio(audioSource, 1.5f, startVolume));
        }
    }

    void ProcessRotation()
    {
        playerRb.freezeRotation = true;
        if(Input.GetKey(KeyCode.D))
        {
            //if (!(transform.rotation.z < -30.0f))
            //{
                transform.Rotate(Vector3.back * rotationThrust * Time.deltaTime);
            //}
            playerRb.AddRelativeForce(Vector3.right * rotationControlThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (transform.rotation.z < 30.0f)
            {
                transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
            }
            playerRb.AddRelativeForce(Vector3.left * rotationControlThrust * Time.deltaTime);
        }
        playerRb.freezeRotation = false;
    }
}
