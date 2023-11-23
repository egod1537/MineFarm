using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using UnityEngine;
using DG.Tweening;
using GoogleSheet.Protocol.v2.Req;
using UnityEngine.UI;
using TMPro;

namespace Minefarm.InGame.UI.Bottom
{
    public class ViewPlayerExp : MonoBehaviour
    {
        public Transform expBar;
        public TextMeshProUGUI txtExp;

        PlayerModel playerModel { get => GameManager.ins.player; }
        Image imgExp;

        private void Awake()
        {
            imgExp = expBar.GetComponent<Image>();
            playerModel.onExp.AddListener(exp => UpdateExp(exp));
        }
        private void Start()
        {
            playerModel.onSpawn.AddListener(() => UpdateExp(playerModel.exp));
            UpdateExp(playerModel.exp);
        }
        public void UpdateExp(long exp)
        {
            long nextExp = playerModel.GetNextLevelExp();
            txtExp.text = $"{exp.ToString("#,##0")} / {nextExp.ToString("#,##0")}";

            float ratio = Mathf.Max(0f, 1.0f * exp / nextExp);

            imgExp.DOKill();
            imgExp.DOFillAmount(ratio, 0.25f);
            imgExp.DOColor(Color.gray, 0.125f)
                .OnComplete(() => imgExp.DOColor(Color.white, 0.125f));
        }
    }
}
