//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/InputSystem/PlayerInputAsset.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputAsset: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAsset()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAsset"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""19f1c46b-1f39-4c23-9741-0b22f5e0de59"",
            ""actions"": [
                {
                    ""name"": ""MenuScene"",
                    ""type"": ""Button"",
                    ""id"": ""a263cbb9-e80e-48cd-b976-ad3e9c9650a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PlayScene"",
                    ""type"": ""Button"",
                    ""id"": ""a4fb1a44-ddea-40aa-9bcb-5f4a49df7159"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MiscScene"",
                    ""type"": ""Button"",
                    ""id"": ""887011e9-e908-40aa-82ea-aef8623c370d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""618596ad-a8dd-46e6-a405-c653654d3e17"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d67d813-eee0-4d44-a282-94559ead8986"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""288e2053-25e1-45dc-867a-dcc3bdea8f4d"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiscScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MenuScene = m_Player.FindAction("MenuScene", throwIfNotFound: true);
        m_Player_PlayScene = m_Player.FindAction("PlayScene", throwIfNotFound: true);
        m_Player_MiscScene = m_Player.FindAction("MiscScene", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_MenuScene;
    private readonly InputAction m_Player_PlayScene;
    private readonly InputAction m_Player_MiscScene;
    public struct PlayerActions
    {
        private @PlayerInputAsset m_Wrapper;
        public PlayerActions(@PlayerInputAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuScene => m_Wrapper.m_Player_MenuScene;
        public InputAction @PlayScene => m_Wrapper.m_Player_PlayScene;
        public InputAction @MiscScene => m_Wrapper.m_Player_MiscScene;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @MenuScene.started += instance.OnMenuScene;
            @MenuScene.performed += instance.OnMenuScene;
            @MenuScene.canceled += instance.OnMenuScene;
            @PlayScene.started += instance.OnPlayScene;
            @PlayScene.performed += instance.OnPlayScene;
            @PlayScene.canceled += instance.OnPlayScene;
            @MiscScene.started += instance.OnMiscScene;
            @MiscScene.performed += instance.OnMiscScene;
            @MiscScene.canceled += instance.OnMiscScene;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MenuScene.started -= instance.OnMenuScene;
            @MenuScene.performed -= instance.OnMenuScene;
            @MenuScene.canceled -= instance.OnMenuScene;
            @PlayScene.started -= instance.OnPlayScene;
            @PlayScene.performed -= instance.OnPlayScene;
            @PlayScene.canceled -= instance.OnPlayScene;
            @MiscScene.started -= instance.OnMiscScene;
            @MiscScene.performed -= instance.OnMiscScene;
            @MiscScene.canceled -= instance.OnMiscScene;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMenuScene(InputAction.CallbackContext context);
        void OnPlayScene(InputAction.CallbackContext context);
        void OnMiscScene(InputAction.CallbackContext context);
    }
}
