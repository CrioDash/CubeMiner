using System.Collections;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class PowerUpgradeScript:MonoBehaviour
    {
        [SerializeField] private Variables.PowerType _type;
        [SerializeField] private GameObject _levelContainer;
        [SerializeField] private GameObject _levelPrefab;
        
        private Button _btn;
        private ParticleSystem _system;
        private TextMeshProUGUI _text;

        private int _currentCost;

        private void Awake()
        {
            _btn = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _system = _levelContainer.transform.parent.GetComponentInChildren<ParticleSystem>();
            for(int i = 0; i< PlayerSave.Instance.powerupLevels[_type]; i++)
                Instantiate(_levelPrefab, _levelContainer.transform);
        }

        private void Start()
        {
            
            _btn.onClick.AddListener(() => StartCoroutine(UpgradeRoutine()));
            if(PlayerSave.Instance.powerupLevels[_type]<5)
                UpdateCost();
        }

        private void Update()
        {
            _btn.interactable = PlayerSave.Instance.Money >= _currentCost
                                && PlayerSave.Instance.powerupLevels[_type]<5;
            _text.text = PlayerSave.Instance.powerupLevels[_type]<5 ? _currentCost.ToString() : "MAX";
        }

        IEnumerator UpgradeRoutine()
        {
            float t = 0;
            GameObject gm = Instantiate(_levelPrefab, transform.parent);
            gm.transform.localScale = Vector3.one * 50;
            
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = _levelContainer.transform.position;

            while (t < 1)
            {
                gm.transform.localScale = Vector3.Lerp(Vector3.one * 50, Vector3.one, t);
                gm.transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
                t += Time.deltaTime*4;
            }

            gm.transform.localScale = Vector3.one;
            gm.transform.position = endPos;

            gm.transform.SetParent(_levelContainer.transform);

            yield return null;
            
            _system.transform.position = gm.transform.position;
            _system.Play();

            PlayerSave.Instance.Money -= _currentCost;
            PlayerSave.Instance.powerupLevels[_type]++;
            if(PlayerSave.Instance.powerupLevels[_type]<5)
                UpdateCost();
            _text.text = _currentCost.ToString();
        }

        void UpdateCost()
        {
            _currentCost = 2000 * (PlayerSave.Instance.powerupLevels[_type] + 1);
        }
        
    }
}