using Minefarm.Map.Block;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Entity.Actor.Player
{
    public class PlayerController : ActorController
    {
        public PlayerModel playerModel { get=> actorModel as PlayerModel; }
        public override void Awake()
        {
            base.Awake();

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => base.Jump());

            this.UpdateAsObservable()
                .Select(_ =>
                {
                    float dx = Input.GetAxis("Horizontal"), dy = Input.GetAxis("Vertical");
                    Vector3 ret = new Vector3(dx + dy, 0f, dy - dx);
                    if (ret.sqrMagnitude > 1f) ret.Normalize();
                    return ret;
                })
                .Where(dir => !Mathf.Approximately(dir.sqrMagnitude, 0f))
                .Subscribe(dir => base.Move(dir));

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => FowardAction());

            playerModel.onDig.AddListener((block, damage) => Debug.Log($"Break : {block.blockID} {damage}"));
        }

        public void Dig(BlockModel block, int damage)
        {
            if (playerModel.digable.Dig(block, damage))
                playerModel.onDig.Invoke(block, damage);
        }
    }
}

