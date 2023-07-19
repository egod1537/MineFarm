using Minefarm.Entity.Actor.Monster;
using Minefarm.Entity.Actor.NPC;
using Minefarm.Entity.Actor.Player;
using Minefarm.Entity.Bullet;
using Minefarm.Map.Block;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Minefarm.Entity.Actor.FowardActionable
{
    public class PlayerFowardActionable : Actorable, IFowardActionable
    {
        protected PlayerController playerController { get => controller as PlayerController; }

        public PlayerFowardActionable(ActorModel actor) : base(actor)
        {
        }

        public bool Action(EntityModel target)
        {
            EntityModel fowardEntity = controller.GetFowardEntity();
            if (fowardEntity == null) controller.Shoot(actor.body.transform.forward);
            else
            {
                switch (fowardEntity)
                {
                    case BlockModel:
                        playerController.Dig(
                            fowardEntity as BlockModel, actor.FormulateAttack());
                        break;
                    case MonsterModel:
                        controller.Shoot(actor.body.transform.forward);
                        break;
                    case NPCModel:
                        Debug.Log("NPC");
                        break;
                }
            }
            return true;
        }
    }
}