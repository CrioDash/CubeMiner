using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BlockInfo", menuName = "Gameplay/New BlockInfo")]
    public class BlockInfo: ScriptableObject
    {
        [SerializeField] private int _reward;
        [SerializeField] private int _health;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AudioClip _clip;

        public int Reward => _reward;
        public int Health => _health;
        public Sprite Sprite => _sprite;
        public AudioClip Clip => _clip;

    }
}