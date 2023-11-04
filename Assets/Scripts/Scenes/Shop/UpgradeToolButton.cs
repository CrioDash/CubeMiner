using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class UpgradeToolButton:MonoBehaviour
    {
        [SerializeField] private Image _shovel_image;

        private Button _btn;
        private ParticleSystem _system;
        private TextMeshProUGUI _text;

        private int _currentCost;

        private void Awake()
        {
            _btn = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _system = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            
            _btn.onClick.AddListener(() => StartCoroutine(ShovelUpgradeRoutine()));
            if(PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]<3)
                UpdateCost();
            _shovel_image.sprite =
                Variables.ToolInfo[PlayerSave.Instance.CurrentTool][PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]]
                    .Sprite;
            _btn.interactable = PlayerSave.Instance.Money >= _currentCost
                                && PlayerSave.Instance.ToolLevel[Variables.ToolType.Shovel] < 3;
            _text.text = PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]<3 ? _currentCost.ToString() : "MAX";
        }

        void UpdateCost()
        {
            _currentCost =
                Variables.ToolInfo[PlayerSave.Instance.CurrentTool][
                    PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool] + 1].Cost;
        }

        private IEnumerator ShovelUpgradeRoutine()
        {
            float t = 0;
            Image gm = Instantiate(_shovel_image.gameObject, transform.parent).GetComponent<Image>();
            gm.transform.localScale = Vector3.one * 50;
            gm.sprite = Variables.ToolInfo[PlayerSave.Instance.CurrentTool][PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]+1]
                .Sprite;
            while (t < 1)
            {
                gm.transform.localScale = Vector3.Lerp(Vector3.one * 50, Vector3.one, t);
                yield return null;
                t += Time.deltaTime*4;
            }
            
            Destroy(gm);
            
            _system.Play();
            _shovel_image.sprite = gm.sprite;

            PlayerSave.Instance.Money -= _currentCost;
            PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]++;
            if(PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]<3)
                UpdateCost();
            _text.text = _currentCost.ToString();
            _btn.interactable = PlayerSave.Instance.Money >= _currentCost
                                && PlayerSave.Instance.ToolLevel[Variables.ToolType.Shovel] < 3;
            _text.text = PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]<3 ? _currentCost.ToString() : "MAX";
            Variables.UpdateVariables();
        }
        
    }
}