using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustForce = 0.0f;
    [SerializeField] float rotationThrust = 0.0f;
    [SerializeField] float rotationControlThrust = 0.0f;
    Rigidbody playerRb;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
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
            Debug.Log("Key pressed");
            playerRb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
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
