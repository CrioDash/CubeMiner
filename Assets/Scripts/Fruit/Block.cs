using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fruit
{
    public class Block: MonoBehaviour, ISlice
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        private List<Vector2> points = new List<Vector2>();

        private bool IsSliced = false;

        private Bounds bounds;

        private SpriteRenderer _renderer;
        private BoxCollider _collider;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider>();
            bounds = _renderer.bounds;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            points.Add(transform.InverseTransformPoint(other.transform.position));
            if (points.Count != 2)
            {
                points = new List<Vector2>();
                return;
            }
            Slice();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            points = new List<Vector2>();
            points.Add(transform.InverseTransformPoint(other.transform.position));
        }

        public void Slice()
        {
            if(IsSliced)
                return;
            IsSliced = true;
            SpriteCutter.Instance.Cut(points, gameObject);
        }
    }
}