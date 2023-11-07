using System;
using System.Collections;
using PowerUps;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Variables = Data.Variables;

namespace Fruit
{
    public class Chest: MonoBehaviour
    {
        [SerializeField] private Sprite spriteChestOpened;
        [SerializeField] private Transform powerSpawnPos;
        [SerializeField] private AnimationCurve animationCurve;

        private AudioSource _audioSource;
        private SpriteRenderer _sprite;
        private ParticleSystem _system;
        private Rigidbody2D _body;

        private bool isOpened = false;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _body = GetComponent<Rigidbody2D>();
            _system = GetComponentInChildren<ParticleSystem>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnMouseUpAsButton()
        {
            OpenChest();
        }

        private void OnMouseUp()
        {
            OpenChest();
        }

        void OpenChest()
        {
            if(isOpened)
                return;

            isOpened = true;
            _audioSource.Play();
            
            _sprite.sprite = spriteChestOpened;
            _system.Play();
            transform.eulerAngles = Vector3.zero;
            _body.bodyType = RigidbodyType2D.Static;
            _body.totalForce = Vector2.zero;

            PowerUpManager.Instance.CreatePowerUp(transform.position, powerSpawnPos.position);

            StartCoroutine(ChestRotateRoutine());
            StartCoroutine(ChestFadeRoutine());

        }


        private IEnumerator ChestRotateRoutine()
        {
            float t = 0;

            Vector3 eulerAngle = transform.eulerAngles;
            
            float startRotation = transform.eulerAngles.z - 15;
            float endRotation = transform.eulerAngles.z + 15;

            while (true)
            {
                t = 0;

                while (t<1)
                {
                    eulerAngle.z = Mathf.Lerp(startRotation, endRotation, animationCurve.Evaluate(t));
                    transform.eulerAngles = eulerAngle;
                    t += Time.deltaTime*2;
                    yield return null;
                }

                t = 0;

                while (t<1)
                {
                    eulerAngle.z = Mathf.Lerp(endRotation, startRotation, animationCurve.Evaluate(t));
                    transform.eulerAngles = eulerAngle;
                    t += Time.deltaTime*2;
                    yield return null;
                }
            }
        }

        private IEnumerator ChestFadeRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            Color startClr = _sprite.color;
            Color endClr = startClr;
            endClr.a = 0;
            
            float t = 0;

            while (t < 1)
            {
                _sprite.color = Color.Lerp(startClr, endClr, t);
                t += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }

    }
}