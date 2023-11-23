using Minefarm.Entity;
using Minefarm.Entity.Actor;
using Unity.VisualScripting;
using UnityEngine;

namespace Minefarm.Map.Structure
{
    public class MonsterSpawner : MonoBehaviour
    {
        public float radius;
        public int minQuantity, maxQuantity;
        public float period;
        public float timer;
        public EntityID spawnEntity;

        private void Update()
        {
            if(timer >= period)
                SpawnEntity();
            timer += Time.deltaTime;
        }

        public void SpawnEntity()
        {
            int quantity = Random.Range(minQuantity, maxQuantity);
            for(int i=0; i < quantity; i++)
            {
                float r = Mathf.Sqrt(Random.Range(0f, radius));
                float t = Random.Range(0f, 360f) * Mathf.Deg2Rad;

                Vector3 dvec = r * new Vector3(Mathf.Cos(t), 0.5f, Mathf.Sin(t));

                ActorModel actor = 
                    Spawner.CreateActor(spawnEntity, transform.position + dvec);            
            }

            timer = 0f;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gray;

            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}