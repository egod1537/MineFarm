using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/CustomEffectComponent", typeof(UniversalRenderPipeline))]
public class CustomEffectComponent : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter dx = 
        new ClampedFloatParameter(value :0, min:-1, max:1);
    public ClampedFloatParameter dy =
        new ClampedFloatParameter(value: 0, min: -1, max: 1);
    public ClampedFloatParameter intercept =
        new ClampedFloatParameter(value: 0, min: 0, max: 1);

    public ClampedFloatParameter slash =
        new ClampedFloatParameter(value : 0, min : 0, max : 0.25f);
    public ClampedFloatParameter decompose =
        new ClampedFloatParameter(value : 0, min : 0, max : 1);

    // Other 'Parameter' variables you might have

    // Tells when our effect should be rendered
    public bool IsActive() => slash.value > 0;

    // I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
