using Minefarm.Entity;
using Minefarm.Item;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public void Action()
    {
        Spawner.DropItem(transform.position, ItemID.Grass);
    }
}
