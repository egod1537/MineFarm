using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Minefarm.Effect
{
    public class EffectDamage : MonoBehaviour
    {        
        public Transform body;

        public float destroyTime;
        public Ease ease;

        public int damage;

        public void Start()
        {
            Destroy(gameObject, destroyTime);
            transform.DOMoveY(transform.position.y + 1, 0.5f).SetEase(ease);

            TextMeshPro text = body.GetComponent<TextMeshPro>();
            text.text = damage.ToString();
            text.DOColor(Color.clear, 0.5f).SetDelay(0.5f);
        }
    }
}