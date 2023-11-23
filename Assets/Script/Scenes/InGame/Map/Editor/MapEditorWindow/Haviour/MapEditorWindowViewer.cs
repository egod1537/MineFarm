using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Minefarm.Map.Window
{
    public class MapEditorWindowViewer : MapEditorWindowHaviour
    {
        public MapEditorWindowViewer(MapEditorWindow window) : base(window)
        {
        }
        
        public override void OnSceneGUI(SceneView sceneView)
        {
            if (!IsActive()) return;
            DrawGrid(sceneView);
        }

        private void DrawGrid(SceneView sceneView)
        {
            if (!IsActive()) return;
            Handles.BeginGUI();

            Handles.SetCamera(sceneView.camera);

            Handles.color = Color.white;
            Handles.matrix = transform.ToMat();

            Vector3 normalCamera = sceneView.camera.transform.forward;

            void Draw(Vector3 pivot, Matrix4x4 mat)
            {
                Vector3 column0 = mat.GetColumn(0);
                Vector3 column1 = mat.GetColumn(1);

                int n = map.size.Dot(column0.ToVector3Int());
                int m = map.size.Dot(column1.ToVector3Int());

                for (int i = 0; i <= n; i++)
                    Handles.DrawLine(column0 * i + pivot, column0 * i + column1 * m + pivot);
                for (int i = 0; i <= m; i++)
                    Handles.DrawLine(column1 * i + pivot, column0 * n + column1 * i + pivot);
            }
            void DrawWithCulling(Matrix4x4 mat)
            {
                Vector3 column2 = mat.GetColumn(2);
                int k = map.size.Dot(column2.ToVector3Int());

                if (Vector3.Dot(normalCamera, -column2) > 0f)
                    Draw(Vector3.zero, mat);
                if (Vector3.Dot(normalCamera, column2) > 0f)
                    Draw(k * column2, mat);
            }

            DrawWithCulling(Matrix4x4.identity);
            DrawWithCulling(ExMatrix4x4.YZX);
            DrawWithCulling(ExMatrix4x4.ZXY);

            Handles.EndGUI();
        }
    }
}

