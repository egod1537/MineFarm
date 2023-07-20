using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Minefarm.Map;
using Minefarm.Entity.Actor.Player;

namespace Minefarm.InGame
{
    public class GameManager : Singletone<GameManager>
    {
        public static float time;

        public MapModel map;
        public PlayerModel player;

        public void Awake()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => time += Time.deltaTime);
        }
    }
}
