using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgParallax : MonoBehaviour
{

    /// <summary>
    /// Parallax background script
    /// Does not work very well right now
    /// </summary>

    public Camera mainCam;
    public float parallaxValue;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = mainCam.transform.position * parallaxValue;
        relativePos.z = startPosition.z;
        transform.position = startPosition + relativePos;

    }
}
