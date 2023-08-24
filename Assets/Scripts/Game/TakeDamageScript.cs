﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
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
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.TAKE_DAMAGE, TakeDamage);
        }

        private void TakeDamage()
        {

            if (Variables.CurrentHealth == Variables.MaxHealth)
            {
                SceneSwitcher.Instance.LoadScene("MenuScene");
                return;
            }
            
            Color clr = Color.white;
            clr.a = 0.3f;
            
            _image.color = clr;
            _image.sprite = Sprites[Variables.CurrentHealth];
            
            Variables.CurrentHealth++;
            
            Debug.Log("Damaged: " + Variables.CurrentHealth);
           
            UpdateRestoring();
            
            _source.Play();
        }

        private void ReleafDamage()
        {
            Variables.CurrentHealth -= 1;
            
            Debug.Log("Healed: " + Variables.CurrentHealth);

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