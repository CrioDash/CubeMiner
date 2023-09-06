using Data;
using UnityEngine;

namespace PowerUps
{
    public abstract class PowerUp:MonoBehaviour
    {
        [SerializeField] private int _duration;
        [SerializeField] private Variables.PowerType _type;

        public int Duration => _duration;
        public Variables.PowerType Type => _type;

        public static int PowerUpCount = 2;
        
        public abstract void UsePowerUp();

        public abstract void RemovePowerUp();
    }
}