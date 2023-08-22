using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fruit
{
    public class Block: MonoBehaviour, ISlice
    {
        [SerializeField] private int MaxHealth;

        private int CurrentHealth;
        
        private List<Vector2> points = new List<Vector2>();
        

        private Bounds bounds;

        private Rigidbody2D _body;
        private ParticleSystem _particleSystem;
        private SpriteRenderer _renderer;
        private BoxCollider _collider;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider>();
            bounds = _renderer.bounds;
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.CompareTag("Player"))
                return;
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
            if (other.CompareTag("Barrier"))
                Destroy(gameObject);
            if(!other.CompareTag("Player"))
                return;
            points = new List<Vector2>();
            points.Add(transform.InverseTransformPoint(other.transform.position));

            float angle = Mathf.Atan2(other.transform.position.y - transform.position.y,
                other.transform.position.x - transform.position.x);
            
            DealDamage(Variables.Damage, angle);
        }

        public void Slice()
        {
            if (CurrentHealth <= 0)
                SpriteCutter.Instance.Cut(points, gameObject);
        }

        public void DealDamage(int dmg, float angle)
        {
            
            CurrentHealth -= dmg;
            if (CurrentHealth > 0)
            {
                _body.AddForce(new Vector2(Random.Range(750, 1500)*-Mathf.Cos(angle), Random.Range(1250, 2000)));
                _body.AddTorque(1, ForceMode2D.Impulse);
            }
        }
    }
}