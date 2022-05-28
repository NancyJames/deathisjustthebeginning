using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;
using UnityEngine.Events;

namespace HR.Utilities.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundFXManager : MonoBehaviour
    {
        [SerializeField] SoundFXEvent[] events;
        AudioSource myAudio;
        Listener[] listeners;

        [System.Serializable]
        public struct SoundFXEvent
        {
            public GameEvent gameEvent;
            public AudioClip clip;

        }

        private void Awake()
        {
            myAudio = GetComponent<AudioSource>();
            listeners = new Listener[events.Length];

        }

        private void OnEnable()
        {
            for(int i =0;i<events.Length;i++)
            {
                listeners[i] = new Listener();
                listeners[i].Response = new UnityEvent<object>();
                listeners[i].Event = events[i].gameEvent;
                AudioClip clip = events[i].clip;
                listeners[i].Response.AddListener(delegate { PlaySound(clip); });
                events[i].gameEvent.Subscribe(listeners[i]);
            }
        }

        public void PlaySound(AudioClip clip)
        {
            myAudio.PlayOneShot(clip);
        }

        private void OnDisable()
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].gameEvent.Unsubscribe(listeners[i]);
            }
        }

    }
}

