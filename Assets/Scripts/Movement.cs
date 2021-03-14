using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustForce = 0.0f;
    [SerializeField] float rotationThrust = 0.0f;
    [SerializeField] float rotationControlThrust = 0.0f;
    [SerializeField] ParticleSystem leftThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

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
            
            if(!leftThrustParticles.isPlaying)
            {
                leftThrustParticles.Play();
            }
            if(!rightThrustParticles.isPlaying)
            {
                rightThrustParticles.Play();
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.W) && audioSource.isPlaying)
            {
                FadeOutAudioEffect();
            }
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            if (leftThrustParticles.isPlaying)
            {
                leftThrustParticles.Stop();
            }
            if (rightThrustParticles.isPlaying)
            {
                rightThrustParticles.Stop();
            }
        }
    }

    void FadeOutAudioEffect()
    {
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
            StopCoroutine(fadeOutCouroutine);
            fadeOutCouroutine = null;
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
            if (!leftBoosterParticles.isPlaying)
            {
                leftBoosterParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (transform.rotation.z < 30.0f)
            {
                transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
            }
            playerRb.AddRelativeForce(Vector3.left * rotationControlThrust * Time.deltaTime);
            if (!rightBoosterParticles.isPlaying)
            {
                rightBoosterParticles.Play();
            }
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            if(leftBoosterParticles.isPlaying)
            {
                leftBoosterParticles.Stop();
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (rightBoosterParticles.isPlaying)
            {
                rightBoosterParticles.Stop();
            }
        }
        playerRb.freezeRotation = false;
    }
}
