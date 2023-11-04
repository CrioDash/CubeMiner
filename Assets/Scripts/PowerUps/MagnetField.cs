using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace PowerUps
{
    public class MagnetField:MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;


        private void Awake()
        {
            
        }

        private void Start()
        {
            StartCoroutine(MagnetRotateRoutine());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Fruit"))
                StartCoroutine(BlockMagnetRoutine(col.GetComponent<Rigidbody2D>()));
        }

        private IEnumerator BlockMagnetRoutine(Rigidbody2D body)
        {
            Vector3 startPos = body.position;
            float t = 0;

            while (body !=null)
            {
                t = 0;
                startPos = body.position;
                while (t < 1)
                {
                    if(body == null || body.GetComponent<BoxCollider2D>() == null || !body.GetComponent<BoxCollider2D>().enabled )
                        yield break;
                    transform.position = Vector3.zero;
                    body.MovePosition(Vector3.Lerp(startPos, this.transform.position, _curve.Evaluate(t)));
                    yield return null;
                    t += Time.deltaTime;
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
        
        private IEnumerator MagnetRotateRoutine()
        {
            float t = 0;
            
            float minAngle = 65;
            float maxAngle = 115;
            
            Vector3 startVec = Vector3.zero;
            Vector3 endVec = Vector3.zero;
            
            startVec.z = minAngle;
            endVec.z = maxAngle;
            while (true)
            {
                t = 0;
                while (t < 1)
                {
                    
                    transform.eulerAngles = Vector3.Lerp(startVec, endVec, t);
                    t += Time.deltaTime * 4;
                    yield return null;
                }

                t = 0;
                while (t <1)
                {
                    transform.eulerAngles = Vector3.Lerp(endVec, startVec, t);
                    t += Time.deltaTime * 4;
                    yield return null;
                }
            }
        }
    }
}