using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Minefarm.UI
{
    public class ViewDashBar : MonoBehaviour
    {
        Image[] bars;
        
        PlayerModel playerModel { get => GameManager.ins.player; }

        public void Awake()
        {
            int sz = PlayerModel.MAX_DASH_COUNT;
            bars = new Image[sz];
            for (int i=0; i < PlayerModel.MAX_DASH_COUNT; i++)
                bars[i] = transform.GetChild(i).GetComponent<Image>();

            playerModel.onDash.AddListener(() =>
            {
                int cnt = playerModel.dashCount-1;
                bars[cnt + 1].DOFillAmount(0f, 0.25f);
                bars[cnt + 1].DOColor(Color.gray, 0.125f)
                    .OnComplete(() => bars[cnt+1].DOColor(Color.white, 0.125f));
            });

            playerModel.onDashCharged.AddListener(() =>
            {
                int cnt = playerModel.dashCount-1;
                bars[cnt].DOFillAmount(1f, 0.25f);
                bars[cnt].DOColor(Color.gray, 0.125f)
                    .OnComplete(() => bars[cnt].DOColor(Color.white, 0.125f));
            });
        }
    }
}

