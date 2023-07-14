using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
namespace Minefarm.InGame
{
    public class GameManager : Singletone<GameManager>
    {
        public static float time;

        public void Awake()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => time += Time.deltaTime);
        }
    }
}
