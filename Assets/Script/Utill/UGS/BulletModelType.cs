using GoogleSheet.Type;
using Minefarm.Entity.Bullet;
using Minefarm.Item;
using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(BulletModelType), "BulletModelType")]
    public class BulletModelTypeType : IType
    {
        public object DefaultValue => BulletModelType.Melee;

        public object Read(string value) => Enum.Parse(typeof(BulletModelType), value);

        public string Write(object value) => $"{value}";
    }
}