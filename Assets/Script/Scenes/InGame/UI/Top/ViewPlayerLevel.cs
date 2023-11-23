using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using TMPro;
using UnityEngine;

namespace Minefarm.InGame.UI
{
    public class ViewPlayerLevel : MonoBehaviour
    {
        public TextMeshProUGUI txtLevel;
        PlayerModel playerModel { get => GameManager.ins.player; }
        private void Awake()
        {
            playerModel.onLevel.AddListener(level => UpdateLevel(level));
        }
        private void Start()
        {
            UpdateLevel(playerModel.level);
        }
        public void UpdateLevel(int level)
        {
            txtLevel.text = $"{level}";
        }
    }
}