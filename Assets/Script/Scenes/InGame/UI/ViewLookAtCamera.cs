using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.InGame.UI
{
    public class ViewLookAtCamera : MonoBehaviour
    {
        Camera camera;
        private void Awake()
        {
            camera = Camera.main;
        }
        private void Update()
        {
            transform.LookAt(camera.transform.position);
        }
    }
}