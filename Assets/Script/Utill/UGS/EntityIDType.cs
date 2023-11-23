using GoogleSheet.Type;
using Minefarm.Entity;
using Minefarm.Item;
using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(EntityID), "EntityID")]
    public class EntityIDType : IType
    {
        public object DefaultValue => EntityID.Slime;

        public object Read(string value) => Enum.Parse(typeof(EntityID), value);

        public string Write(object value) => $"{value}";
    }
}