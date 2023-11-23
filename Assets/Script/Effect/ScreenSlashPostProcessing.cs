using DG.Tweening;
using Minefarm.InGame.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScreenSlashPostProcessing : MonoBehaviour
{
    public Volume postProcessing;
    public float speedSlash;
    public float delayDecompose;
    public float speedDecompose;

    Animator slashAnimator;
    CustomEffectComponent effect;
    private void Awake()
    {
        slashAnimator = GetComponent<Animator>();
        postProcessing.profile.TryGet(out effect);
    }

    public void Run()
    {
        effect.slash.value = 0f;
        effect.decompose.value = 0f;
        slashAnimator.Play("Slash");
    }

    Coroutine ca, cb;
    public void Callback()
    {
        if(ca != null)StopCoroutine(ca);
        if(cb != null)StopCoroutine(cb);
        WaitForSeconds wfs = new WaitForSeconds(speedSlash);
        IEnumerator a()
        {
            while(effect.slash.value < 0.25f)
            {
                yield return null;
                effect.slash.value += Time.deltaTime*speedSlash;
            }
        }
        ca = StartCoroutine(a());

        IEnumerator b()
        {
            Debug.Log("hi");
            yield return new WaitForSeconds(delayDecompose);
            while (effect.decompose.value < 0.05f)
            {
                yield return null;
                effect.decompose.value = Mathf.Lerp(
                    effect.decompose.value, 
                    0.05f, 
                    Time.deltaTime*speedDecompose);
            }
        }
        cb = StartCoroutine(b());
    }
}
