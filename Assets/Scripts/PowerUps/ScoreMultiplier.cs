using Data;
using UnityEngine;

namespace PowerUps
{
    public class ScoreMultiplier:PowerUp
    {
        public override void UsePowerUp()
        {
            Variables.ScoreMultiplier = 2;
        }

        public override void RemovePowerUp()
        {
            Variables.ScoreMultiplier = 1;
        }
    }
}