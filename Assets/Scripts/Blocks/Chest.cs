using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Fruit
{
    public class Chest: MonoBehaviour
    {
        [SerializeField] private Sprite spriteChestOpened;
        [SerializeField] private AnimationCurve animationCurve;

        private SpriteRenderer _sprite;
        private ParticleSystem _system;
        private Rigidbody2D _body;

        private bool isOpened = false;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _body = GetComponent<Rigidbody2D>();
            _system = GetComponentInChildren<ParticleSystem>();
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
            
            _sprite.sprite = spriteChestOpened;
            _system.Play();
            _body.bodyType = RigidbodyType2D.Static;
            _body.totalForce = Vector2.zero;

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
                    transform.eulerAngles = eulerAngle;
                    eulerAngle.z = Mathf.Lerp(endRotation, startRotation, animationCurve.Evaluate(t));
                    t += Time.deltaTime*2;
                    yield return null;
                }
            }
        }

        private IEnumerator ChestFadeRoutine()
        {
            yield return new WaitForSeconds(1f);
            
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