using GoogleSheet.Type;
using Minefarm.Entity.Actor.Block;
using Minefarm.Entity.Bullet;
using Minefarm.Item;
using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(BlockID), "BlockID")]
    public class BlockIDType : IType
    {
        public object DefaultValue => BlockID.Air;

        public object Read(string value) => Enum.Parse(typeof(BlockID), value);

        public string Write(object value) => $"{value}";
    }
}