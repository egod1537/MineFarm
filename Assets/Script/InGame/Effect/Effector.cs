using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Minefarm.Effect
{
    public class Effector : Singletone<Effector>
    {
        const float DELAY_CONTINUOUS_DAMAGE = 0.1f;
        const float POS_Y_CONTINUOUS_DAMAGE = 0.75f;

        public static EffectDamage CreateDamage(Vector3 pos, int damage, VertexGradient vertexGradient)
        {
            GameObject go = Instantiate(EffectDB.LoadDamage());

            Transform tr = go.transform;
            tr.SetParent(ins.transform);
            tr.position = pos + Vector3.up;

            EffectDamage ret = go.GetComponent<EffectDamage>();
            ret.damage = damage;

            TextMeshPro tmp = ret.body.GetComponent<TextMeshPro>();
            tmp.colorGradient = vertexGradient;

            return ret;
        }
        public static EffectDamage CreateDamage(Vector3 pos, int damage)
            => CreateDamage(pos, damage, new VertexGradient());

        public static void CreateDamages(
            Vector3 pos, 
            List<int> damages,
            List<VertexGradient> vertexGradients)
        {
            if (damages.Count == 0) return;

            WaitForSeconds wfs = new WaitForSeconds(DELAY_CONTINUOUS_DAMAGE);

            IEnumerator iterator()
            {
                for(int i=0; i < damages.Count; i++)
                {
                    pos.y += POS_Y_CONTINUOUS_DAMAGE;
                    CreateDamage(pos, damages[i], vertexGradients[i]);
                    yield return wfs;
                }
            }
            ins.StartCoroutine(iterator());
        }
    }
}