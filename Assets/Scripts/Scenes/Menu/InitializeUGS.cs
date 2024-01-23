using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Scenes.Menu
{
    public class InitializeUgs : MonoBehaviour
    {
        
        
        public string environment = "production";
 
        async void Start() {
        
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);
 
            await UnityServices.InitializeAsync(options);

        }
    }
}
