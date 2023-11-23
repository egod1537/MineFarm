using Minefarm.Entity.Actor.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Minefarm.Item.Actionable
{
    public interface IItemActionable
    {
        public void DrawButton(ItemModel item, TextMeshProUGUI label);

        public void Action(ItemModel item, PlayerModel player);
    }
}

