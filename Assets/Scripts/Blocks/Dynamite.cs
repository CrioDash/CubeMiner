using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using EventBus = Utilities.EventBus;
using Variables = Data.Variables;

namespace Fruit
{
    public class Dynamite:MonoBehaviour, ISlice
    {
        [SerializeField] private int explosionPower;
        [SerializeField] private int explosionRadius;

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                Slice();
        }
        
        public void Slice()
        {
            StartCoroutine(ExplodeRoutine());
        }

        private void OnMouseUpAsButton()
        {
            Explode();
        }

        private void OnMouseDown()
        {
            Explode();
        }

        private void Explode()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            Debug.Log(colliders.Length);
            
            foreach (Collider2D col in colliders)
            {
                if (!col.CompareTag("Fruit"))
                    continue;
                float angle = Mathf.Atan2(col.transform.position.y - transform.position.y,
                    col.transform.position.x - transform.position.x);
                col.GetComponent<Block>().ExplosiveDamage(explosionPower, angle);
            }
            
            ParticleSystem system = GetComponentInChildren<ParticleSystem>();
            system.transform.SetParent(null);
            system.transform.localScale = Vector3.one;
            system.Play();

            EventBus.Publish(EventBus.EventType.CHANGE_BLOCK);
            Variables.BlocksCut = 0;
            
            Destroy(gameObject);
        }

        private IEnumerator ExplodeRoutine()
        {
            Variables.CurrentHealth = 0;
            PauseScript.SetPause();
            float t = 0;
            while (t<6)
            {
                EventBus.Publish(EventBus.EventType.TAKE_DAMAGE);
                yield return new WaitForSecondsRealtime(0.35f);
                t += 1;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}