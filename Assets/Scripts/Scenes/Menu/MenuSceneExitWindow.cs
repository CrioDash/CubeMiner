using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MenuSceneExitWindow: MonoBehaviour
    {
        public static MenuSceneExitWindow Instance;

        [SerializeField] private Material referenceMat;
        [SerializeField] private CanvasGroup btnGroup;
        
        private bool _isVisible = false;
        
        private Image[] _images;
        private Material[] _referenceCopies;
        private CanvasGroup _group;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Instance = this;
            _group.alpha = _isVisible ? 1 : 0;
            _group.blocksRaycasts = _isVisible;
            _images = GetComponentsInChildren<Image>();
            _referenceCopies = new Material[_images.Length];
            for (int i = 0; i < _images.Length; i++)
            {
                _referenceCopies[i] = new Material(referenceMat.shader);
                _images[i].material = _referenceCopies[i];
                _images[i].material.SetTexture("_Texture2D", _images[i].sprite.texture);
            }
        }

        public void ChangeWindowState()
        {
            _isVisible = !_isVisible;
            if (_isVisible)
                gameObject.SetActive(true);
            StartCoroutine(_isVisible ? ShowPageRoutine() : CloseOptionsRoutine());
        }
        
        private IEnumerator ShowPageRoutine()
        {
            btnGroup.interactable = false;
            _group.alpha = 1;
            float t = 0;
            while (t< 1)
            {
                for (int i = 0; i < _images.Length; i++)
                {
                    _images[i].material.SetFloat("_Dissolve", Mathf.Lerp(1, -0.1f, t));
                }
                t += Time.unscaledDeltaTime*4f;
                yield return null;
            }

            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].material.SetFloat("_Dissolve", -0.1f);
            }
            _group.blocksRaycasts = true;
            
        }
        
        private IEnumerator CloseOptionsRoutine()
        {
            float t = 0;
            while (t< 1)
            {
                for (int i = 0; i < _images.Length; i++)
                {
                    _images[i].material.SetFloat("_Dissolve", Mathf.Lerp(-0.1f, 1, t));
                }
                t += Time.unscaledDeltaTime*4;
                yield return null;
            }
            _group.alpha = 0;
            _group.blocksRaycasts = false;
            btnGroup.interactable = true;
            gameObject.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
        
    }
}