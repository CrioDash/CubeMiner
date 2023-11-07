using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

namespace PowerUps
{
    public class PowerUpManager:MonoBehaviour
    {
        [SerializeField] private GameObject[] powerUpPrefab;
        [SerializeField] private GameObject powerUpContainer;
        [SerializeField] private AnimationCurve animationCurve;
        
        public static PowerUpManager Instance;
        
        
        private Dictionary<Variables.PowerType, CanvasGroup> powerUps = new Dictionary<Variables.PowerType, CanvasGroup>();
        private Dictionary<Variables.PowerType, Coroutine> activePowerUps =
            new Dictionary<Variables.PowerType, Coroutine>();

        private void Start()
        {
            Instance = this;
        }

        public void CreatePowerUp(Vector3 chestPos, Vector3 startPos)
        {
            StartCoroutine(PowerUpRoutine(chestPos, startPos));
        }
        
        private IEnumerator PowerUpRoutine(Vector3 chestPos, Vector3 startPos)
        {
            Variables.PowerType type = (Variables.PowerType) Random.Range(0, powerUpPrefab.Length);
            
            float t = 0;

            CanvasGroup group = new CanvasGroup();
            Vector3 endPos = Vector3.zero;

           

            if (activePowerUps.ContainsKey(type))
            {
                group = powerUps[type];
            }
            else
            {
                group = Instantiate(powerUpPrefab[(int) type], powerUpContainer.transform)
                    .GetComponent<CanvasGroup>();
                group.alpha = 0;
            }
            
            GameObject movingIcon = Instantiate(group.transform.GetChild(0).gameObject, startPos, Quaternion.identity);
            movingIcon.transform.SetParent(group.transform.parent);
            

            while (t <= 1)
            {
                while (PauseScript.IsPaused)
                    yield return null;
                movingIcon.transform.position = Vector3.Lerp(chestPos, startPos, animationCurve.Evaluate(t));
                movingIcon.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(t));
                t += Time.deltaTime * 4;
                yield return null;
            }
            
            t = 0;
            
            endPos = group.transform.position;

            while (t <= 1)
            {
                while (PauseScript.IsPaused)
                    yield return null;
                movingIcon.transform.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(t));
                t += Time.deltaTime * 4;
                yield return null;
            }

            if (group == null)
            {
                group = Instantiate(powerUpPrefab[(int) type], powerUpContainer.transform)
                    .GetComponent<CanvasGroup>();
                group.alpha = 0;
            }
           
            group.alpha = 1;
            Destroy(movingIcon.gameObject);
            List<CanvasGroup> groups = powerUpContainer.transform.GetComponentsInChildren<CanvasGroup>().ToList();
            if(groups.Find(x => x.name == group.name && groups.IndexOf(x) < groups.IndexOf(group)))
            {
                CanvasGroup temp = group;
                group = powerUpContainer.transform.GetComponentsInChildren<CanvasGroup>().ToList()
                    .Find(x => x.name == temp.name);
                Destroy(temp.gameObject);
            }

            if (activePowerUps.ContainsKey(type))
            {
                StopCoroutine(activePowerUps[type]);
                activePowerUps[type] = StartCoroutine(FadePowerUp(type, group, group.GetComponent<PowerUp>().Duration));
            }
            else
            {
                powerUps.Add(type, group);
                activePowerUps.Add(type, StartCoroutine(FadePowerUp(type, group, group.GetComponent<PowerUp>().Duration)));
            }
            
            powerUps[type].GetComponent<PowerUp>().UsePowerUp();

        }

        private IEnumerator FadePowerUp(Variables.PowerType type, CanvasGroup group, float duration)
        {
            float t = 0;
            while (t < duration)
            {
                while (PauseScript.IsPaused)
                    yield return null;
                group.alpha = Mathf.Lerp(1, 0.2f, t / duration);
                t += Time.deltaTime;
                yield return null;
            }

            group.alpha = 0;
            
            group.GetComponent<PowerUp>().RemovePowerUp();

            yield return new WaitForSeconds(1f);
            Destroy(group.gameObject);
            powerUps.Remove(type);
            activePowerUps.Remove(type);
            

        }

        
        
    }
}