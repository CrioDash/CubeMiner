using System;
using System.Collections;
using Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using EventBus = Utilities.EventBus;
using Variables = Data.Variables;

namespace UI
{
    public class EndGamePanelScript:MonoBehaviour
    {
        [SerializeField] private AnimationCurve animCurve;
        
        [SerializeField] private GameObject panelEndGame;
        [SerializeField] private Image panelBack;
        [SerializeField] private Transform spotMark;
        [SerializeField] private Image imageMark;
        [SerializeField] private TextMeshProUGUI textScore;
        [SerializeField] private TextMeshProUGUI textMoney;
        [SerializeField] private TextMeshProUGUI textBlocks;
        [SerializeField] private TextMeshProUGUI textRecord;
        [SerializeField] private TextMeshProUGUI textAddMoney;
        [SerializeField] private TextMeshProUGUI textBonusMoney;
        [SerializeField] private ParticleSystem particleBonus;
        [SerializeField] private Sprite[] MarkList;

        
        private Button _btnOk; 
        private CanvasGroup _group;
        private static Coroutine _coroutine;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
            _btnOk = GetComponentInChildren<Button>();
            
        }

        private void Start()
        {
            _btnOk.onClick.AddListener(() =>  SceneSwitcher.Instance.LoadScene("MenuScene"));
            _group.alpha = 0;
            _group.blocksRaycasts = false;
            imageMark.enabled = false;
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_END, ShowEngGame);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_END, ShowEngGame);
        }

        private void ShowEngGame()
        {
            if(_coroutine == null)
                _coroutine = StartCoroutine(ShowEndGameRoutine());
        }
        
        private IEnumerator ShowEndGameRoutine()
        {
            if(!PauseScript.IsPaused)
                PauseScript.SetPause();
            
            

            #region Variables

            float t = 0;
            
            string add;
            int score = 0;
            
            Color endClr = panelBack.color;
            Color startClr = endClr;
            startClr.a = 0;

            Vector3 startPos = panelEndGame.transform.position;
            Vector3 endPos = Vector3.zero;

            panelBack.color = Color.clear;
            _group.alpha = 1;
            _group.blocksRaycasts = true;
            
            int markScore = 0;

            Variables.BestCombo = Mathf.Clamp(Variables.BestCombo, 0, 5);
            
            markScore += Variables.BestCombo > 2 ? Variables.BestCombo - 2 : 0;
            
            markScore += Mathf.RoundToInt(Mathf.Clamp(((float)Variables.BlocksCut / Variables.BlocksFall) - 0.7f, 0, 1) * 10);

            textBlocks.text = "0/" + Variables.BlocksFall;

                add = "";
            for (int i = 0; i < 6 - PlayerSave.Instance.Money.ToString().Length; i++)
                add += "0";
            
            textMoney.text = add + PlayerSave.Instance.Money;
            
            int markBonus = Mathf.RoundToInt((float)markScore* Variables.Money / 20);

            PlayerSave.Instance.Money += Variables.Money + markBonus;
            PlayerSave.Instance.Money = Mathf.Clamp(PlayerSave.Instance.Money, 0, 999999);
            
            
            int startMoney = PlayerSave.Instance.Money- Variables.Money - markBonus;
            int endMoney = PlayerSave.Instance.Money - markBonus;
            
            int moneyShow = startMoney;

            #endregion
            
            #region MovePanel

            
            
            while (t<1)
            {
                panelEndGame.transform.position = Vector3.Lerp(startPos, endPos,animCurve.Evaluate(t));
                panelBack.color = Color.Lerp(startClr, endClr, t);
                t += Time.unscaledDeltaTime*3/2;
                yield return null;
            }

            panelEndGame.transform.position = endPos;
            panelBack.color = endClr;

            #endregion

            yield return new WaitForSecondsRealtime(0.15f);

            #region CountScore

            t = 0;
                
            while (t < 1)
            {
                if(Variables.Score==0)
                    break;
                score = Mathf.RoundToInt(Mathf.Lerp(0, Variables.Score, t));
                add = "";
                for (int i = 0; i < 8 - score.ToString().Length; i++)
                    add += "0";
                textScore.text = add + score;
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }
            
            add = "";
            for (int i = 0; i < 8 - Variables.Score.ToString().Length; i++)
                add += "0";

            textScore.text = add + Variables.Score;

                #endregion
                
            yield return new WaitForSecondsRealtime(0.15f);

            #region RecordCheck

            if (Variables.Score > PlayerSave.Instance.RecordScore)
            {
                PlayerSave.Instance.RecordScore = Variables.Score;
                
                startClr = textRecord.color;
                endClr = startClr;
                endClr.a = 1;
                
                for (int i = 0; i < 3; i++)
                {
                    t = 0;
                    while (t<1)
                    {
                        textRecord.color = Color.Lerp(startClr, endClr, t);
                        t += Time.unscaledDeltaTime * 2;
                        yield return null;
                    }
                    t = 0;
                    while (t<1)
                    {
                        textRecord.color = Color.Lerp(endClr, startClr, t);
                        t += Time.unscaledDeltaTime * 2;
                        yield return null;
                    }
                }
                

                textRecord.color = startClr;
            }

            #endregion

            #region MoneyScore

            startClr = textAddMoney.color;
            endClr = startClr;
            endClr.a = 1;
            
           

            textAddMoney.text = "+" + Variables.Money;

            t = 0;
            
            while (t < 1)
            {
                textAddMoney.color = Color.Lerp(startClr, endClr, t);
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }

            textAddMoney.color = endClr;

            t = 0;
            
            while (t < 1)
            {
                if(Variables.Score==0)
                    break;
                moneyShow = Mathf.RoundToInt(Mathf.Lerp(startMoney, endMoney, t));
                add = "";
                for (int i = 0; i < 6 - moneyShow.ToString().Length; i++)
                    add += "0";
                textMoney.text = add + moneyShow;
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }

            moneyShow = endMoney;
            
            add = "";
            for (int i = 0; i < 6 - moneyShow.ToString().Length; i++)
                add += "0";

            textMoney.text = add + moneyShow;
            
            t = 0;
            
            while (t < 1)
            {
                textAddMoney.color = Color.Lerp(endClr, startClr, t);
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }

            textAddMoney.color = startClr;

                #endregion

            yield return new WaitForSecondsRealtime(0.15f);

            #region BlocksScore

            t = 0;
            
            while (t < 1)
            {
                textBlocks.text = Mathf.RoundToInt(Mathf.Lerp(0, Variables.BlocksCut, t)) + "/" + Variables.BlocksFall; 
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }

            textBlocks.text = Variables.BlocksCut + "/" + Variables.BlocksFall;
            
                #endregion
            
            yield return new WaitForSecondsRealtime(0.15f);

            #region CountMarkScore

            t = 0;

            imageMark.transform.position = Vector3.zero;
            imageMark.transform.localScale = Vector3.one * 15;
            
            imageMark.sprite = MarkList[markScore];
            
            imageMark.enabled = true;

            startPos = imageMark.transform.position;
            endPos = spotMark.transform.position;

            while (t<1)
            {
                imageMark.transform.position = Vector3.Lerp(startPos, endPos,animCurve.Evaluate(t));
                imageMark.transform.localScale = Vector3.Lerp(Vector3.one * 15, Vector3.one, animCurve.Evaluate(t));
                t += Time.unscaledDeltaTime*3;
                yield return null;
            }
            
            imageMark.transform.position = endPos;
            imageMark.transform.localScale = Vector3.one;

                #endregion

            yield return new WaitForSecondsRealtime(0.15f);

            #region BonusMoney

            if(markBonus==0)
                yield break;
            
            t = 0;

            textBonusMoney.text = "+" + markBonus;
            
            startClr = textBonusMoney.color;
            startClr.a = 1;
            textBonusMoney.color = startClr;

            startPos = textBonusMoney.transform.position;
            endPos = textMoney.transform.position;

            while (t < 1)
            {
                textBonusMoney.transform.position = Vector3.Slerp(startPos,endPos, animCurve.Evaluate(t));
                t += Time.unscaledDeltaTime*2f;
                yield return null;
            }

            textBonusMoney.transform.position = endPos;
            
            textBonusMoney.color = Color.clear;

            particleBonus.Play();

            t = 0;

            startMoney = Int32.Parse(textMoney.text);
            endMoney = startMoney + markBonus;

            while (t < 1)
            {
                moneyShow = Mathf.RoundToInt(Mathf.Lerp(startMoney, endMoney, t));
                add = "";
                for (int i = 0; i < 6 - moneyShow.ToString().Length; i++)
                    add += "0";
                textMoney.text = add + moneyShow;
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }

            moneyShow = endMoney;
            add = "";
            for (int i = 0; i < 6 - moneyShow.ToString().Length; i++)
                add += "0";
            textMoney.text = add + moneyShow;

            #endregion

        }
        
    }
}