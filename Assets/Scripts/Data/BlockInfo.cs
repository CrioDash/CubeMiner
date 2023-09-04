using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BlockInfo", menuName = "Gameplay/New BlockInfo")]
    public class BlockInfo: ScriptableObject
    {
        [SerializeField] private int _rewardScore;
        [SerializeField] private int _rewardMoney;
        [SerializeField] private int _health;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AudioClip _clip;

        public int RewardScore => _rewardScore;
        public int MoneyReward => _rewardMoney;
        public int Health => _health;
        public Sprite Sprite => _sprite;
        public AudioClip Clip => _clip;

    }
}