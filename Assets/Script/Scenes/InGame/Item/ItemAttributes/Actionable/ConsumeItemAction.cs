using Minefarm.Entity.Actor.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Minefarm.Item.Actionable
{
    public class ConsumeItemAction : IItemActionable
    {
        public void Action(ItemModel item, PlayerModel player)
        {
            throw new System.NotImplementedException();
        }

        public void DrawButton(ItemModel item, TextMeshProUGUI label)
        {
            label.text = "»ç¿ë";
        }
    }
}

