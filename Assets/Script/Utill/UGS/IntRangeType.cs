using GoogleSheet.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hamster.ZG.Type
{
    [Type(typeof(IntRange), "IntRange")]
    public class IntRangeType : IType
    {
        public object DefaultValue => new IntRange(0);

        public object Read(string value)
        {
            string[] str = value.Split('~');
            if (str.Length == 1) return new IntRange(int.Parse(value));
            return new IntRange(int.Parse(str[0]), int.Parse(str[1]));
        }

        public string Write(object value)
        {
            IntRange range = (IntRange)value;
            return $"{range.l}~{range.r}";
        }
    }
}