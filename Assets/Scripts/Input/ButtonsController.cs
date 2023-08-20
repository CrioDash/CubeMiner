using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
            PlayerInputAsset.Player.MiscScene.performed += OtherScenesBackButton;

            LastAction = PlayerInputAsset.FindAction(SceneManager.GetActiveScene().name);

            foreach (string s in ControlsData.ActionsList)
            {
                if(s == LastAction.name)
                    continue;
                PlayerInputAsset.FindAction(s).Disable();
                Debug.Log(PlayerInputAsset.FindAction(s));
            }
        }
        

        #region ControlMethods
        
        public void PlaySceneBackButton(InputAction.CallbackContext context)
        {
            PlayScenePauseWindow.Instance.ChangeWindowState();
        }
        
        public void MenuSceneBackButton(InputAction.CallbackContext context)
        {
            MenuSceneExitWindow.Instance.ChangeWindowState();
        }
        
        public void OtherScenesBackButton(InputAction.CallbackContext context)
        {
            SceneSwitcher.Instance.LoadScene("MenuScene");
        }
        
        #endregion

        public void ResetControls(string action)
        {
           if(LastAction!=null)
               LastAction.Disable();
           LastAction = PlayerInputAsset.FindAction(action);
           LastAction.Enable();
        }
        
    }
}