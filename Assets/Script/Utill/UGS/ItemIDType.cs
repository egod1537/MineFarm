using GoogleSheet.Type;
using Minefarm.Item;
using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(ItemID), "ItemID")]
    public class ItemIDType : IType
    {
        public object DefaultValue => ItemID.Air;

        public object Read(string value) => Enum.Parse(typeof(ItemID), value);

        public string Write(object value) => $"{value}";
    }
}