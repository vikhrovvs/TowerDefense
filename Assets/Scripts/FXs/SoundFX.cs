using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FXs
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundFX : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_AudioClip;

        [SerializeField, Range(-3f, 3f)]
        private float m_MinPitch;
        [SerializeField, Range(-3f, 3f)]
        private float m_MaxPitch;
        private void Awake()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = m_AudioClip;
            audioSource.pitch = Random.Range(m_MinPitch, m_MaxPitch); //1 - оригинальная высота
            
            audioSource.Play();
        }
    }
}