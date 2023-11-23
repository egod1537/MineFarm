using UnityEngine;
using UnityEditor;
namespace Minefarm.Entity.EditorInsepctor
{
    [CustomEditor(typeof(EntityModel), true)]
    public class EditorEntityModel : Editor
    {
        public bool isEntityFold;

        protected EntityModel script;
        public virtual void OnEnable()
        {
            script = (EntityModel)target;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isEntityFold = EditorGUILayout.Foldout(
                    isEntityFold, "[Etntiy Infomation]");
                if (isEntityFold)
                {
                    script.entityID =
                        (EntityID)EditorGUILayout.EnumPopup("Entity ID", script.entityID);
                    script.group =
                        (EntityGroup)EditorGUILayout.EnumPopup("Entity Group", script.group);
                    script.isLive = EditorGUILayout.Toggle("Is Live", script.isLive);
                    EditorGUILayout.Vector3IntField("Map Pos", script.mapPos);

                    script.body = (Transform)EditorGUILayout.ObjectField(
                        "Body", script.body, typeof(Transform), true);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(script, $"{typeof(EntityModel)} {script.name}");
            PrefabUtility.RecordPrefabInstancePropertyModifications(this.script);
        }
    }
}
