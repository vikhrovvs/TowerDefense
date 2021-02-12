using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    private const float CameraSpeedMultiplier = 5;
    [SerializeField] private GameObject targetObject;
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 cameraPosition = 
            _camera.ScreenToWorldPoint(
                new Vector3(_camera.pixelWidth, _camera.pixelHeight, transform.position.z)
                );
        //if (cameraPosition.magnitude > MaxDeviation) return;

        Vector3 objectPosition = targetObject.transform.position;
        if ((Mathf.Abs(objectPosition.x) > Mathf.Abs(cameraPosition.x))
            || (Mathf.Abs(objectPosition.y) > Mathf.Abs(cameraPosition.y)))
        {
            transform.Translate(Vector3.back * (CameraSpeedMultiplier * Time.deltaTime));
        }
    }
}