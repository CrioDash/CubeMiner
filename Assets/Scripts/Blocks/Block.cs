using System;
using System.Collections.Generic;
using Data;
using Game;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;
using BlockType = Data.Variables.BlockType;

namespace Fruit
{
    public class Block: MonoBehaviour, ISlice
    {
        private int MaxHealth;
        private int CurrentHealth;
        private int Reward;

        private List<Vector2> points = new List<Vector2>();

        private Rigidbody2D _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
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
            if (other.CompareTag("Barrier") && CurrentHealth > 0)
            {
                EventBus.Publish(EventBus.EventType.TAKE_DAMAGE);
                Destroy(gameObject);
            }

            if(!other.CompareTag("Player"))
                return;
            points = new List<Vector2>();
            points.Add(transform.InverseTransformPoint(other.transform.position));

            float angle = Mathf.Atan2(other.transform.position.y - transform.position.y,
                other.transform.position.x - transform.position.x);
            
            DealDamage(angle);
        }

        public void Slice()
        {
            if (CurrentHealth <= 0)
            {
                Variables.Score += Reward;
                Variables.BlocksCut++;
                SpriteCutter.Instance.CutFruit(points, gameObject);
            }
        }

        public void DealDamage(float angle)
        {
            
            CurrentHealth -= Variables.Damage;
            if (CurrentHealth > 0)
            {
                _body.AddForce(new Vector2(Random.Range(750, 1500)*-Mathf.Cos(angle), Math.Abs(1500*Mathf.Sin(angle))));
                _body.AddTorque(1, ForceMode2D.Impulse);
            }
        }
        
        public void ExplosiveDamage(int expPower, float angle)
        {
            
            CurrentHealth -= 1000;
            _body.AddForce(new Vector2(expPower*Mathf.Cos(angle), expPower*Mathf.Sin(angle)));
            _body.AddTorque(1, ForceMode2D.Impulse);
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject,2f);
        }

        public void SetStats(BlockType type)
        {
            BlockInfo info = Variables.BlockInfo[type];
            MaxHealth = info.Health;
            CurrentHealth = MaxHealth;
            Reward = info.Reward;
            GetComponent<SpriteRenderer>().sprite = info.Sprite;
            GetComponentInChildren<AudioSource>().clip = info.Clip;
        }
    }
}