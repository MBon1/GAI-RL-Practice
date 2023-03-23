using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropellerX : MonoBehaviour
{
    public float rotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        // rotate the propeller model
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
