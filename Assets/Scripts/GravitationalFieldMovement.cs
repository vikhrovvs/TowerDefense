using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalFieldMovement: MonoBehaviour
{
    [SerializeField] private float mass;
    [SerializeField] private Vector3 speed;
    [SerializeField] private Vector3 position;
    [SerializeField] private float g;

    private void Start()
    {
        transform.position = position;
    }

    private void FixedUpdate()
    {
        Application.targetFrameRate = 60; //frames per second
        float deltaTime = Time.deltaTime; //согласно документации, это правильная deltaTime в любом случае
        //float deltaTime = 1 / (float)Application.targetFrameRate;
        ////так точно правильно, но менее красиво, поэтому хочу использовать Time.deltaTime, если это корректно
        Vector3 position = transform.position; //Почему-то на это warning (local variable hides serialised field), но мне кажется, что так наоборот правильнее
        Vector3 acceleration = -1 * g * mass * position / (float) Math.Pow(position.magnitude, 3);
        float max_acceleration = 25; //ограничение, чтобы не улетать на бесконечность
        if (acceleration.magnitude > max_acceleration)
        {
            acceleration = acceleration.normalized * max_acceleration;
        }
        Vector3 delta = speed * deltaTime + acceleration * (deltaTime * deltaTime) / 2;
        speed += acceleration * deltaTime;
        transform.Translate(delta);
    }
}
