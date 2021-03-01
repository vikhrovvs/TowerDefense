using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class Runner : MonoBehaviour
    {
        private List<IController> m_Controllers;
        private bool m_IsRunning = false;
        private void Update()
        {
            if (!m_IsRunning)
            {
                return;
            }
            TickControllers();
        }

        public void StartRunning()
        {
            CreateAllControllers();
            OnStartControllers();
            m_IsRunning = true;
        }

        public void StopRunning()
        {
            m_IsRunning = false;
            OnStopControllers();
        }

        private void CreateAllControllers()
        {
            m_Controllers = new List<IController>();
            m_Controllers.Add(new TestController());
        }
        private void OnStartControllers()
        {
            foreach (IController controller in m_Controllers)
            {
                try
                {
                    controller.OnStart();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        
        private void TickControllers()
        {
            foreach (IController controller in m_Controllers)
            {
                try
                {
                    controller.Tick();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        
        private void OnStopControllers()
        {
            foreach (IController controller in m_Controllers)
            {
                try
                {
                    controller.OnStop();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}