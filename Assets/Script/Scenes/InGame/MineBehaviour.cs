using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehaviour : MonoBehaviour
{
    protected PlayerModel player { get => GameManager.ins.player; }
}
