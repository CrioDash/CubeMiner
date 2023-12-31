﻿using System;
using System.Collections.Generic;
using Data;
using Game;
using PlayerScripts;
using UI;
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
        private int RewardScore;
        private int RewardMoney;
        private BlockType Type;

        private List<Vector2> points = new List<Vector2>();

        private Rigidbody2D _body;
        private GameObject _childCollider;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _childCollider = transform.GetChild(0).gameObject;
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
                Variables.Score += RewardScore*Variables.ScoreMultiplier;
                Variables.Score = Mathf.Clamp(Variables.Score, 0, 99999999);

                MoneySpriteScript.Instance.CreateMoney(RewardMoney*Variables.ScoreMultiplier, transform.position);
                
                BlockSpawner.BlocksCut++;
                Variables.BlocksCut++;
                
                ParticleSystem system = GetComponentInChildren<ParticleSystem>();
                
                system.transform.SetParent(null);
                
                Vector3 pos = transform.position;
                pos.z -= 1;
                pos.y -= 1;
                system.transform.position = pos;
                system.transform.localScale = Vector3.one;
                
                system.gameObject.GetComponent<AudioSource>().Play();
        
                system.gameObject.GetComponent<ParticleSystemRenderer>().material = GetComponent<SpriteRenderer>().material;
                system.Play();


                ComboTextScript.comboCount++;
                SpriteCutter.Instance.CutFruit(points, gameObject);
            }
        }

        public void DealDamage(float angle)
        {
            
            CurrentHealth -= Variables.Damage;
            if (CurrentHealth > 0)
            {
                _body.AddForce(new Vector2(Random.Range(250, 350)*-Mathf.Cos(angle), Math.Abs(750*Mathf.Sin(angle))));
                _body.AddTorque(1, ForceMode2D.Impulse);
            }
        }
        
        public void ExplosiveDamage(int expPower, float angle)
        {
            _childCollider.SetActive(false);
            CurrentHealth -= 1000;
            _body.AddForce(new Vector2(expPower*Mathf.Cos(angle), expPower*Mathf.Sin(angle)));
            _body.AddTorque(1, ForceMode2D.Impulse);
            
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject,2f);
        }

        public void SetStats(BlockType type)
        {
            BlockInfo info = Variables.BlockInfo[type];
            Type = type;
            MaxHealth = info.Health;
            CurrentHealth = MaxHealth;
            RewardScore = info.RewardScore;
            RewardMoney = info.MoneyReward;
            GetComponent<SpriteRenderer>().sprite = info.Sprite;
            GetComponentInChildren<AudioSource>().clip = info.Clip;
        }
    }
}