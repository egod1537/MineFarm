using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Minefarm.InGame.Util
{
    public class PostProcessingVolume : Singletone<PostProcessingVolume>
    {
        private static Volume _volume;
        protected static Volume volume { get => _volume ??= ins.GetComponent<Volume>(); }

        private static DepthOfField _depthOfField;
        protected static DepthOfField depthOfField
        {
            get
            {
                if(_depthOfField == null)
                    volume.profile.TryGet(out _depthOfField);
                return _depthOfField;
            }
        }

        public static void SetDepthOfField(float value)
        {
            depthOfField.gaussianEnd.SetValue(new MinFloatParameter(value, 0f));
        }
        public static void SetDepthOfFieldActive(bool flag)
        {
            depthOfField.active = flag;
        }
    }
}