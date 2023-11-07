using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace UI
{
    public class ShopSceneButtonScript:MonoBehaviour
    {
        [SerializeField] private Light2D[] lights;
        
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private CanvasGroup containerGroup;
        [SerializeField] private CanvasGroup btnGroup;

        [SerializeField] private GameObject gmShopImage;

        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;
        
        [SerializeField] private string sceneName;

        private Button _button;
        private Animator _shopAnimator;
        private ParticleSystem _system;
        
        
        private void Awake()
        {
            _shopAnimator = gmShopImage.GetComponentInChildren<Animator>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => StartCoroutine(BookOpenCoroutine()));
        }

        private void Start()
        {
            containerGroup.alpha = 0;
            containerGroup.blocksRaycasts = false;
            if (SceneSwitcher.LastScene == "ShopScene")
                StartCoroutine(BookCloseCoroutine());
        }

        private IEnumerator BookOpenCoroutine()
        {
            btnGroup.interactable = false;
            containerGroup.alpha = 1;
            containerGroup.blocksRaycasts = true;
            
            float t = 0;
            while (t < 1)
            {
                gmShopImage.transform.position =
                    Vector3.Lerp(startPos.position, Vector3.zero, animationCurve.Evaluate(t));
                gmShopImage.transform.localScale =
                    Vector3.Lerp(Vector3.one*9, Vector3.one, animationCurve.Evaluate(t));
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }
            
            _shopAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(1f);

            foreach (Light2D light in lights)
            {
                light.volumeIntensity = 0;
            }

            SceneSwitcher.Instance.LoadScene("ShopScene");

            t = 0;
            while (t < 1)
            {
                gmShopImage.transform.position =
                    Vector3.Lerp(Vector3.zero, endPos.position, animationCurve.Evaluate(t));
                gmShopImage.transform.localScale =
                    Vector3.Lerp(Vector3.one, Vector3.one*9, animationCurve.Evaluate(t));
                t += Time.unscaledDeltaTime*1.5f;
                yield return null;
            }
        }
        
        private IEnumerator BookCloseCoroutine()
        {
            btnGroup.interactable = false;
            containerGroup.alpha = 1;
            containerGroup.blocksRaycasts = true;
            
            _shopAnimator.SetTrigger("Close");
            
            float t = 0;
            while (t < 1)
            {
                gmShopImage.transform.position =
                    Vector3.Lerp(startPos.position, Vector3.zero, animationCurve.Evaluate(t));
                gmShopImage.transform.localScale =
                    Vector3.Lerp(Vector3.one*9, Vector3.one, animationCurve.Evaluate(t));
                t += Time.deltaTime*2;
                yield return null;
            }
            
            
            yield return new WaitForSeconds(1f);

            t = 0;
            while (t < 1)
            {
                gmShopImage.transform.position =
                    Vector3.Lerp(Vector3.zero, Vector3.right*6, animationCurve.Evaluate(t));
                t += Time.deltaTime*1.5f;
                yield return null;
            }
            
            btnGroup.interactable = true;
            containerGroup.alpha = 0;
            containerGroup.blocksRaycasts = false;

        }
    }
}