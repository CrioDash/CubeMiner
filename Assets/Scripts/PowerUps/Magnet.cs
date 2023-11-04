using System;
using Data;
using UnityEngine;

namespace PowerUps
{
    public class Magnet:PowerUp
    {
        private BoxCollider2D _box;

        private void Start()
        {
            _box = GetComponentInChildren<BoxCollider2D>();
            _box.enabled = false;
            Duration += PlayerSave.Instance.powerupLevels[Type] * 1;
        }

        public override void UsePowerUp()
        {
            _box.enabled = true;
            _box.transform.position = Vector3.zero;
        }

        public override void RemovePowerUp()
        {
           
        }
    }
}