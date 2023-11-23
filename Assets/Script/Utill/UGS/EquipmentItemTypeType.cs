using GoogleSheet.Type;
using Minefarm.Entity.Bullet;
using Minefarm.Item;
using Minefarm.Item.Equipment;
using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(EquipmentItemType), "EquipmentItemType")]
    public class EquipmentItemTypeType : IType
    {
        public object DefaultValue => EquipmentItemType.None;

        public object Read(string value) => Enum.Parse(typeof(EquipmentItemType), value);

        public string Write(object value) => $"{value}";
    }
}