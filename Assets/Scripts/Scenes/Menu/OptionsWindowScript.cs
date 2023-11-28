using System;
using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scenes.Menu
{
    public class OptionsWindowScript:MonoBehaviour
    {
        [SerializeField] private CanvasGroup buttonsGroup;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private AudioMixerGroup musicMixer;
        [SerializeField] private AudioMixerGroup soundMixer;
        
        private Image _img;

        private CanvasGroup _group;

        private bool _isOpened = false;

        private Image[] _images;

        private void Awake()
        {
            _images = GetComponentsInChildren<Image>();
            _img = GetComponent<Image>();
            _group = GetComponent<CanvasGroup>();
            musicToggle.onValueChanged.AddListener(delegate
            {
                musicMixer.audioMixer.SetFloat(musicToggle.name, musicToggle.isOn ? 0 : -80);
                PlayerSave.Instance.MusicON = musicToggle.isOn;
            });
            soundToggle.onValueChanged.AddListener(delegate
            {
                soundMixer.audioMixer.SetFloat(soundToggle.name, soundToggle.isOn ? 0 : -80); 
                PlayerSave.Instance.SoundON = soundToggle.isOn;
            });
        }

        private void Start()
        {
            foreach (Image img in _images)
            {
                Material mat = new Material(_img.material.shader);
                img.material = mat;
                img.material.SetTexture("_Texture2D", img.sprite.texture);
            }

            musicToggle.isOn = PlayerSave.Instance.MusicON;
            soundToggle.isOn = PlayerSave.Instance.SoundON;
        }

        public void ShowOptions()
        {
            if (!_isOpened)
                gameObject.SetActive(true);
            StartCoroutine(!_isOpened ? ShowOptionsRoutine() : CloseOptionsRoutine());
            
            _isOpened = !_isOpened;
        }

        private IEnumerator ShowOptionsRoutine()
        {
            buttonsGroup.interactable = _isOpened;
            _group.alpha = 1;
            float t = 0;
            while (t< 1)
            {
                _img.material.SetFloat("_Dissolve", Mathf.Lerp(1, -0.1f, t));
                foreach (Image img in _images)
                {
                    img.material.SetFloat("_Dissolve", Mathf.Lerp(1, -0.1f, t));
                }
                t += Time.deltaTime * 4;
                yield return null;
            }
            
            _img.material.SetFloat("_Dissolve", -0.1f);
            foreach (Image img in _images)
            {
                img.material.SetFloat("_Dissolve", -0.1f);
            }

            _group.interactable = true;
            _group.blocksRaycasts = true;
            
        }
        
        private IEnumerator CloseOptionsRoutine()
        {
            _group.interactable = false;
            float t = 0;
            while (t< 1)
            {
                _img.material.SetFloat("_Dissolve", Mathf.Lerp(-0.1f, 1, t));
                foreach (Image img in _images)
                {
                    img.material.SetFloat("_Dissolve", Mathf.Lerp(-0.1f, 1, t));
                }
                t += Time.deltaTime * 4;
                yield return null;
            }
            
            _img.material.SetFloat("_Dissolve", 1);
            foreach (Image img in _images)
            {
                img.material.SetFloat("_Dissolve", 1);
            }
            _group.alpha = 0;
            buttonsGroup.interactable = !_isOpened;
            _group.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

    }
}