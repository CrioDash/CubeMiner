using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ToolInfo", menuName = "Gameplay/New ToolInfo")]
    [Serializable]
    public class ToolInfo:ScriptableObject
    {
        [SerializeField] private Variables.ToolType _type;
        [SerializeField] private int _cost;
        [SerializeField] private int _damage;
        [SerializeField] private Gradient _colorGradient;
        [SerializeField] private Color _color;
        [SerializeField] private Sprite _sprite;

        public int Damage => _damage;
        public int Cost => _cost;
        public Gradient ColorGradient => _colorGradient;
        public Color Color => _color;
        public Variables.ToolType Type => _type;
        public Sprite Sprite => _sprite;
    }
}