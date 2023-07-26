using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Minefarm.UI.InGame
{
    public class ViewPlayerHp : MonoBehaviour
    {
        public Transform hpBar;
        public TextMeshProUGUI txtHp;

        PlayerModel playerModel { get => GameManager.ins.player; }
        Image imgHp;

        private void Awake()
        {
            imgHp = hpBar.GetComponent<Image>();
            playerModel.onHp.AddListener(exp => UpdateHp(exp));
        }
        private void Start()
        {
            playerModel.onSpawn.AddListener(() => UpdateHp(playerModel.hp));
        }
        public void UpdateHp(int hp)
        {
            int maxHp = playerModel.calculatedMaxHp;
            txtHp.text = $"{hp.ToString("#,##0")} / {maxHp.ToString("#,##0")}";

            float ratio = Mathf.Max(0f, 1.0f * hp / maxHp);

            imgHp.DOKill();
            imgHp.DOFillAmount(ratio, 0.25f);
            imgHp.DOColor(Color.gray, 0.125f)
                .OnComplete(() => imgHp.DOColor(Color.white, 0.125f));
        }
    }
}

