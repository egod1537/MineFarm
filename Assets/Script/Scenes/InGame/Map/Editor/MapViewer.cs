using UnityEditor;
using UnityEngine;
namespace Minefarm.Map
{
    [RequireComponent(typeof(MapModel))]
    public class MapViewer : MonoBehaviour
    {
        MapModel _model;
        MapModel model { get =>_model ??= GetComponent<MapModel>(); }

        public void OnDrawGizmosSelected()
        {
            DrawGrid();
        }

        private void DrawGrid()
        {
            Gizmos.color = Color.white;
            Gizmos.matrix = transform.ToMat();

            Vector3 normalCamera = SceneView.currentDrawingSceneView.camera.transform.forward;

            void Draw(Vector3 pivot, Matrix4x4 mat)
            {
                Vector3 column0 = mat.GetColumn(0);
                Vector3 column1 = mat.GetColumn(1);

                int n = model.size.Dot(column0.ToVector3Int());
                int m = model.size.Dot(column1.ToVector3Int());

                for (int i = 0; i <= n; i++)
                    Gizmos.DrawLine(column0*i+pivot, column0*i+column1*m + pivot);
                for (int i = 0; i <= m; i++)
                    Gizmos.DrawLine(column1 * i + pivot, column0 * n + column1 * i + pivot);
            }
            void DrawWithCulling(Matrix4x4 mat)
            {
                Vector3 column2 = mat.GetColumn(2);
                int k = model.size.Dot(column2.ToVector3Int());

                if (Vector3.Dot(normalCamera, -column2) > 0f)
                    Draw(Vector3.zero, mat);
                if (Vector3.Dot(normalCamera, column2) > 0f)
                    Draw(k * column2, mat);
            }

            DrawWithCulling(Matrix4x4.identity);
            DrawWithCulling(ExMatrix4x4.YZX);
            DrawWithCulling(ExMatrix4x4.ZXY);
        }
    }
}
