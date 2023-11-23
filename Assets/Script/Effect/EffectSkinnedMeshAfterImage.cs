using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;

namespace Minefarm.Effect.InGame
{
    public class EffectSkinnedMeshAfterImage : MonoBehaviour
    {
        public Gradient colorGradient = new Gradient();
        public float delaySpawn;
        public float delayDestroy;

        private Queue<KeyValuePair<float, Material>> queue = new();

        SkinnedMeshRenderer skinnedMeshRenderer;

        public void Awake()
        {
            skinnedMeshRenderer= GetComponent<SkinnedMeshRenderer>();

            float timeSpawn = delaySpawn, startTime = 0f;
            this.UpdateAsObservable()
                .Where(_ => enabled && timeSpawn <= 0f)
                .Subscribe(_ =>
                {
                    Mesh mesh = new Mesh();
                    skinnedMeshRenderer.BakeMesh(mesh);

                    GameObject go = new GameObject("[Effect] After Image");
                    Transform tr = go.transform;
                    tr.position = transform.position;
                    tr.rotation = transform.rotation;
                    tr.localScale = transform.localScale;

                    MeshFilter mf = go.AddComponent<MeshFilter>();
                    mf.mesh = mesh;

                    MeshRenderer mr = go.AddComponent<MeshRenderer>();
                    mr.material = new Material(skinnedMeshRenderer.material);
                    queue.Enqueue(new KeyValuePair<float, Material>(startTime, mr.material));
                    Destroy(go, delayDestroy);

                    timeSpawn = delaySpawn;
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    float dt = Time.deltaTime;

                    while(queue.Count > 0)
                    {
                        var data = queue.Peek();
                        float time = data.Key;
                        if ((time - startTime) >= delayDestroy) queue.Dequeue();
                        else break;
                    }

                    foreach(var data in queue)
                    {
                        float time = data.Key;
                        float ratio = (startTime - time) / delayDestroy;
                        data.Value.SetColor("_BaseColor", colorGradient.Evaluate(ratio));
                    }
                    timeSpawn -= dt;
                    startTime += dt;
                });
        }
    }
}

