using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class ComboTextScript:MonoBehaviour
    {
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private Gradient _gradient710;
        [SerializeField] private Gradient _gradient11;
        
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private float timeToWait;

        private static SlashControlScript _slash;
        
        public static int comboCount = 0;

        private void Start()
        {
            _slash = FindObjectOfType<SlashControlScript>();
            StartCoroutine(ComboCountRoutine());
        }

        private IEnumerator ComboCountRoutine()
        {
            WaitUntil wait = new WaitUntil(() => comboCount != 0);
            WaitForSeconds waitSec = new WaitForSeconds(timeToWait);
            while (true)
            {
                comboCount = 0;
                yield return wait;
                yield return waitSec;

                if (Variables.BestCombo < comboCount)
                    Variables.BestCombo = comboCount;

                if (comboCount < 3)
                {
                    yield return null;
                    continue;
                }
                
                TMP_Text text = Instantiate(textPrefab, spawnPos).GetComponent<TMP_Text>();
                text.transform.position = spawnPos.position;
                text.text = "Combo X" + comboCount + "!";

                switch (comboCount)
                {
                    case 3:
                        text.color = Color.yellow;
                        break;
                    case 4:
                        text.color = Color.cyan;
                        break;
                    case 5:
                        text.color = Color.magenta;
                        break;
                    case 6:
                        text.color = new Color(105/255f, 1, 160/255f, 1);
                        break;
                    
                }

                Variables.Score += comboCount * 100;

               
                
                StartCoroutine(TextFadeRoutine(text));
                
                if(comboCount<7)
                {
                    StartCoroutine(TextGrowRoutine(text.transform));
                    StartCoroutine(TextRotateRoutine(text.transform));
                }
                else if (comboCount is >= 7 and <= 12)
                {
                    StartCoroutine(Combo710Routine(text, _gradient710, 10, 25, true));
                }
                else
                {
                    StartCoroutine(Combo710Routine(text, _gradient11, 5, 5, false));
                }
              
                yield return null;
            }
        }

        IEnumerator Combo710Routine(TMP_Text txt, Gradient gradient, float first, float last, bool sin)
        {
            Mesh mesh;

            Vector3[] vertices;

            while (txt != null)
            {
                txt.ForceMeshUpdate();
                mesh = txt.mesh;
                vertices = mesh.vertices;

                Color[] colors = mesh.colors;

                for (int i = 0; i < txt.textInfo.characterCount; i++)
                {
                    Vector3 offset = Wobble(Time.time + i, first, last, sin) * 50;
                    
                    TMP_CharacterInfo c = txt.textInfo.characterInfo[i];

                    int index = c.vertexIndex;

                    colors[index] = gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * 0.001f, 1f));
                    colors[index + 1] =
                        gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
                    colors[index + 2] =
                        gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
                    colors[index + 3] =
                        gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));

                    vertices[index] += offset;
                    vertices[index + 1] += offset;
                    vertices[index + 2] += offset;
                    vertices[index + 3] += offset;
                    
                }

                mesh.vertices = vertices;
                mesh.colors = colors;
                txt.canvasRenderer.SetMesh(mesh);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

         IEnumerator Combo11Routine(TMP_Text txt)
         { 
             Mesh mesh;

             Vector3[] vertices;

             while (txt != null)
             {
                 txt.ForceMeshUpdate();
                 mesh = txt.mesh;
                 vertices = mesh.vertices;
                 string[] words = txt.text.Split(' ');

                 int lastIndex = 0;

                 Color[] colors = mesh.colors;

                 for (int i = 0; i < words.Length; i++)
                 {
                     Vector3 offset = Wobble(Time.time + i, 10, 10, false) * 50;
                     
                     for (int w = 0; w < words[i].Length; w++)
                     {
                         TMP_CharacterInfo c = txt.textInfo.characterInfo[lastIndex + w];

                         int index = c.vertexIndex;

                         colors[index] = _gradient11.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * 0.001f, 1f));
                         colors[index + 1] =
                             _gradient11.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
                         colors[index + 2] =
                             _gradient11.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
                         colors[index + 3] =
                             _gradient11.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));

                         vertices[index] += offset;
                         vertices[index + 1] += offset;
                         vertices[index + 2] += offset;
                         vertices[index + 3] += offset;
                     }

                     lastIndex += words[i].Length + 1;
                 }

                 mesh.vertices = vertices;
                 mesh.colors = colors;
                 txt.canvasRenderer.SetMesh(mesh);
                 yield return new WaitForSeconds(Time.deltaTime);
             }
          
        }

         Vector2 Wobble(float time, float first, float last, bool sin)
         {
             return sin ? new Vector2(Mathf.Sin(time*Random.Range(first,last)), Mathf.Cos(time*Random.Range(first,last))) : new Vector2(0, Mathf.Cos(time*Random.Range(first,last)));
         }
         
        


        private IEnumerator TextGrowRoutine(Transform obj)
        {
            float t = 0;

            while (t < 1)
            {
                obj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                t += Time.deltaTime * 4;
                yield return null;
            }
            obj.transform.localScale = Vector3.one;
        }

        private IEnumerator TextRotateRoutine(Transform obj)
        {
            float t = 0;

            Vector3 eulerAngle = obj.transform.eulerAngles;
            
            float startRotation = obj.transform.eulerAngles.z - 15;
            float endRotation = obj.transform.eulerAngles.z + 15;

            while (true)
            {
                t = 0;

                while (t<1)
                {
                    if (obj == null)
                        yield break;
                    eulerAngle.z = Mathf.Lerp(startRotation, endRotation, animationCurve.Evaluate(t));
                    obj.transform.eulerAngles = eulerAngle;
                    t += Time.deltaTime*2;
                    yield return null;
                }

                t = 0;

                while (t<1)
                {
                    if (obj == null)
                        yield break;
                    eulerAngle.z = Mathf.Lerp(endRotation, startRotation, animationCurve.Evaluate(t));
                    obj.transform.eulerAngles = eulerAngle;
                    t += Time.deltaTime*2;
                    yield return null;
                }
            }
        }

        private IEnumerator TextFadeRoutine(TMP_Text txt)
        {
            
            
            float t = 0;
            CanvasGroup group = txt.GetComponent<CanvasGroup>();
            while (t<1)
            {
                group.alpha = Mathf.Lerp(1, 0, t);

                t += Time.deltaTime/2;
                yield return null;
            }
            Destroy(txt.gameObject);
        }
        
    }
}