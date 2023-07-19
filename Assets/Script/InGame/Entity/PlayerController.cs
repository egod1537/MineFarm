using Minefarm.Map.Block;
using UniRx;
using UniRx.Triggers;
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
                .Subscribe(_ =>
                {
                    if (!FowardAction()) playerModel.onSwing.Invoke();
                });
        }

        public void Break(BlockModel block, int damage)
        {
            
        }
    }
}

