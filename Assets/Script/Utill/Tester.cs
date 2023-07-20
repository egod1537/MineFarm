using Minefarm.Entity.Actor;
using Minefarm.Entity.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public void Action()
    {
        Droper.DropItem(ItemID.Gold, GetComponent<ActorModel>(), transform.position);
    }
}
