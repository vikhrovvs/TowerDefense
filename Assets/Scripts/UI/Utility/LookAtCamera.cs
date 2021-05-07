using System;
using UnityEngine;

namespace UI.Utility
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform m_CameraTransform;

        private void Awake()
        {
            m_CameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.forward = m_CameraTransform.forward;
        }
    }
}