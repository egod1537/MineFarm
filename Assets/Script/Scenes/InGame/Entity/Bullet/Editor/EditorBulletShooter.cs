using UnityEngine;
using UnityEditor;
namespace Minefarm.Entity.Bullet.EditorInspector
{
    [CustomEditor(typeof(BulletShooter))]
    public class EditorBulletShooter : Editor
    {
        BulletShooter sciprt;
        private void OnEnable()
        {
            sciprt = (BulletShooter)target; 
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Shoot")) sciprt.Shoot();
        }
    }

}