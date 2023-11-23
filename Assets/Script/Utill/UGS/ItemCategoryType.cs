using GoogleSheet.Type;
using Minefarm.Item;
using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(ItemCategory), "ItemCategory")]
    public class ItemCategoryType : IType
    {
        public object DefaultValue => ItemCategory.Block;

        public object Read(string value) => Enum.Parse(typeof(ItemCategory), value);

        public string Write(object value) => $"{value}";
    }
}