using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField] Vector3 movementPos;
    [SerializeField] [Range(0,1)] float movementFactor;
    [SerializeField] float period = 2f;

    private const float Tau = Mathf.PI * 2;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period;
        movementFactor = (Mathf.Sin(cycles * Tau) + 1)/2;
        transform.position = startPos + (movementPos * movementFactor);
    }
}
