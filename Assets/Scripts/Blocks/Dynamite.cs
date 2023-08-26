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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Barrier"))
                Destroy(gameObject);
        }

        public void Slice()
        {
            StartCoroutine(ExplodeRoutine(SpriteCutter.Instance.CutTnt(gameObject)));
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
            Handheld.Vibrate();

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
            
            GetComponentInChildren<AudioSource>().Play();
            
            system.transform.SetParent(null);
            system.transform.localScale = Vector3.one;
            system.Play();

            EventBus.Publish(EventBus.EventType.CHANGE_BLOCK);
            Variables.BlocksCut = 0;
            
            Destroy(gameObject);
        }

        private IEnumerator ExplodeRoutine(GameObject[] objects)
        {
            Variables.CurrentHealth = 0;
            
            AudioSource parentSource = GetComponentInChildren<AudioSource>();
            ParticleSystem parentSystem = GetComponentInChildren<ParticleSystem>();
            WaitForSeconds wait = new WaitForSeconds(0.15f);

            yield return new WaitForSeconds(0.25f);

            foreach (GameObject obj in objects)
            {
                Handheld.Vibrate();
                
                ParticleSystem system = Instantiate(parentSystem.gameObject, obj.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

                obj.GetComponent<MeshRenderer>().enabled = false;
                
                AudioSource source = system.AddComponent<AudioSource>();

                source.clip = parentSource.clip;
                source.volume = 1f;
                source.Play();

                system.transform.localScale /= 3;
                
                system.Play();
                
                EventBus.Publish(EventBus.EventType.TAKE_DAMAGE);

                yield return wait;
            }
            EventBus.Publish(EventBus.EventType.TAKE_DAMAGE);
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}