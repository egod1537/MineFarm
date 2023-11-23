using GoogleSheet.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hamster.ZG.Type
{
    [Type(typeof(FloatRange), "FloatRange")]
    public class FloatRangeType : IType
    {
        public object DefaultValue => new FloatRange(0f);

        public object Read(string value)
        {
            string[] str = value.Split('~');
            if (str.Length == 1) return new FloatRange(float.Parse(value));
            return new FloatRange(float.Parse(str[0]), float.Parse(str[1]));
        }

        public string Write(object value)
        {
            FloatRange range = (FloatRange)value;
            return $"{range.l}~{range.r}";
        }
    }
}

