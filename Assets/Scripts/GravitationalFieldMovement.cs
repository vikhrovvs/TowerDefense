using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalFieldMovement: MonoBehaviour
{
    [SerializeField] private float mass;
    [SerializeField] private Vector3 speed;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private GameObject heavyBody;
    [SerializeField] private float g;
    private Vector3 _heavyBodyPosition = Vector3.zero;
    private const float DeltaTime = 0.02f; //default value
    private const float MaxAcceleration = 25;
    
    private void Start()
    {
        if (heavyBody)
        {
            _heavyBodyPosition = heavyBody.transform.position;
        }
        Time.fixedDeltaTime = DeltaTime;
        transform.position = startingPosition;
    }
    
    private void FixedUpdate()
    {
        var deltaTime = Time.deltaTime;
        Vector3 position = transform.position;
        Vector3 acceleration = -1 * g * mass * (position - _heavyBodyPosition) / (float) Math.Pow((position - _heavyBodyPosition).magnitude, 3);
        if (acceleration.magnitude > MaxAcceleration)
        {
            acceleration = acceleration.normalized * MaxAcceleration;
        }
        Vector3 delta = speed * deltaTime + acceleration * (deltaTime * deltaTime) / 2;
        speed += acceleration * deltaTime;
        transform.Translate(delta);
    }
}
