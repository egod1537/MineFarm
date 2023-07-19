using Minefarm.Entity.Actor;
using Minefarm.Entity.EditorInsepctor;
using UnityEditor;
using UnityEngine;
namespace Minefarm.Entity.Bullet.EditorInspector
{
    [CustomEditor(typeof(BulletModel))]
    public class EditorBulletModel : EditorEntityModel
    {
        public bool isBulletFold;
        protected BulletModel bulletModel { get => base.script as BulletModel; }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isBulletFold = EditorGUILayout.Foldout(isBulletFold, "[Bullet Information]");
                if (isBulletFold)
                {
                    bulletModel.owner =
                        (ActorModel)EditorGUILayout.ObjectField(
                            "Owner", 
                            bulletModel.owner, 
                            typeof(ActorModel), 
                            true);

                    bulletModel.damage = EditorGUILayout.IntField("Damage", bulletModel.damage);
                    bulletModel.distance = EditorGUILayout.FloatField("Distance", bulletModel.distance);

                    bulletModel.direction = EditorGUILayout.Vector3Field("Direction", bulletModel.direction);
                    bulletModel.speed = EditorGUILayout.FloatField("Speed", bulletModel.speed);
                }
                EditorGUI.indentLevel--;    
            }
            EditorGUILayout.EndVertical();
        }
    }
}

