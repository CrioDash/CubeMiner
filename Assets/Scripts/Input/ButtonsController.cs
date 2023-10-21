using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utilities;

namespace Input
{
    public class ButtonsController:MonoBehaviour
    {

        private PlayerInputAsset PlayerInputAsset { set; get; }
        private InputAction LastAction { set; get; }
        
       

        private void Start()
        {
            PlayerInputAsset = new PlayerInputAsset();
            PlayerInputAsset.Enable();
            PlayerInputAsset.Player.MenuScene.performed += MenuSceneBackButton;
            PlayerInputAsset.Player.PlayScene.performed += PlaySceneBackButton;
            PlayerInputAsset.Player.ShopScene.performed += ShopSceneBackButton;
            PlayerInputAsset.Player.SettingsScene.performed += SettingsSceneBackButton;

            LastAction = PlayerInputAsset.FindAction(SceneManager.GetActiveScene().name);

            foreach (string s in ControlsData.ActionsList)
            {
                if(s == LastAction.name)
                    continue;
                PlayerInputAsset.FindAction(s).Disable();
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_PAUSE, DisableControl);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_PAUSE, DisableControl);
        }


        #region ControlMethods

        private void PlaySceneBackButton(InputAction.CallbackContext context)
        {
            PlayScenePauseWindow.Instance.ChangeWindowState();
        }

        private void MenuSceneBackButton(InputAction.CallbackContext context)
        {
            MenuSceneExitWindow.Instance.ChangeWindowState();
        }

        private void ShopSceneBackButton(InputAction.CallbackContext context)
        {
            SceneSwitcher.Instance.LoadScene("MenuScene");
        }
        
        private void SettingsSceneBackButton(InputAction.CallbackContext context)
        {
            SceneSwitcher.Instance.LoadScene("MenuScene");
        }
        
        #endregion

        private void DisableControl()
        {
            if (PauseScript.IsPaused)
                LastAction.Disable();
            else
                LastAction.Enable();
        }

        public void ResetControls(string action)
        {
           if(LastAction!=null)
               LastAction.Disable();
           LastAction = PlayerInputAsset.FindAction(action);
           LastAction.Enable();
        }
        
    }
}