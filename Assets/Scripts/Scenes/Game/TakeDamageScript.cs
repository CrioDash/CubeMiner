using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Utilities;
using EventBus = Utilities.EventBus;
using Variables = Data.Variables;

namespace Game
{
    public class TakeDamageScript:MonoBehaviour
    {
        [SerializeField] private Sprite[] Sprites;

        private Image _image;
        private AudioSource _source;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.TAKE_DAMAGE, TakeDamage);
            EventBus.Subscribe(EventBus.EventType.GAME_END, PauseScript.SetPause);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.TAKE_DAMAGE, TakeDamage);
            EventBus.Unsubscribe(EventBus.EventType.TAKE_DAMAGE, PauseScript.SetPause);
        }

        private void TakeDamage()
        {
            Debug.Log("Damage");
            _source.Play();

            if (Variables.CurrentHealth >= Variables.MaxHealth)
            {
                EventBus.Publish(EventBus.EventType.GAME_END);
                return;
            }
            
            Color clr = Color.white;
            clr.a = 0.3f;
            
            _image.color = clr;
            _image.sprite = Sprites[Variables.CurrentHealth];
            
            Variables.CurrentHealth++;
            
           
            UpdateRestoring();
            
           
        }

        private void ReleafDamage()
        {
            Variables.CurrentHealth -= 1;

            if (Variables.CurrentHealth == 0)
                _image.color = Color.clear;
            else
                _image.sprite = Sprites[Variables.CurrentHealth-1];

            UpdateRestoring();
        }

        private void UpdateRestoring()
        {
            StopAllCoroutines();
            StartCoroutine(HealthRestoreRoutine());
        }

        private IEnumerator HealthRestoreRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(5f);
            while (true)
            {
                if (Variables.CurrentHealth == 0)
                {
                    yield return null;
                    continue;
                }
                yield return wait;
                ReleafDamage();
                yield return null;
            }
        }


    }
}