using System;
using System.Collections;
using Data;
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
            if(!PlayerSave.Instance.TutorialCompleted)
                return;
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

            if (PauseScript.IsPaused)
            {
                PlayerSave.Instance.TutorialCompleted = true;
                PauseScript.SetPause();
            }

            if(!PlayerSave.Instance.TutorialCompleted)
                    return;
            
            Handheld.Vibrate();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            

            foreach (Collider2D col in colliders)
            {
                if (!col.CompareTag("Fruit"))
                    continue;
                Variables.BlocksFall --;
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
            BlockSpawner.BlocksCut = 0;
            
            Destroy(gameObject);
        }

        private IEnumerator ExplodeRoutine(GameObject[] objects)
        {

            Variables.CurrentHealth = 0;
            
            AudioSource parentSource = GetComponentInChildren<AudioSource>();
            ParticleSystem parentSystem = GetComponentInChildren<ParticleSystem>();
            WaitForSeconds wait = new WaitForSeconds(0.15f);

            yield return new WaitForSeconds(0.25f);

            ParticleSystem system = Instantiate(parentSystem.gameObject, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            system.transform.localScale /= 3;
            system.Play();
            AudioSource source = system.AddComponent<AudioSource>();

            source.clip = parentSource.clip;
            source.volume = 1f;
            
            foreach (GameObject obj in objects)
            {
                Handheld.Vibrate();

                obj.GetComponent<MeshRenderer>().enabled = false;
                
                
                source.Play();

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