using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Random = UnityEngine.Random;

namespace Game
{
    public class MusicPlayer:MonoBehaviour
    {

        [SerializeField] private AudioClip[] clips;

        private AudioClip _currentClip;
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _currentClip = clips[Random.Range(0, clips.Length)];
            _source.clip = _currentClip;
            _source.Play();
            StartCoroutine(TrackShuffleRoutine());
        }

        private IEnumerator TrackShuffleRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_source.clip.length);
                _currentClip = clips[Mathf.RoundToInt(Random.Range(0, clips.Length-1))];
                while (_currentClip == _source.clip)
                    _currentClip = clips[Mathf.RoundToInt(Random.Range(0, clips.Length-1))];
                _source.clip = _currentClip;
                _source.Play();
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_PAUSE, delegate
            {
                if(PauseScript.IsPaused) _source.Pause();
                else _source.UnPause();
            });
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_PAUSE, delegate
            {
                if(PauseScript.IsPaused) _source.Pause();
                else _source.UnPause();
            });
        }
    }
}