using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.ItemSet
{
    public class MultipleItemIconCapture : MonoBehaviour
    {
        public ItemIconCapture capture;

        public void MultipleCapture()
        {
            gameObject.SetActiveChild(false);
            int cnt = transform.childCount;
            for(int i=0; i < cnt; i++)
            {
                Transform tr = transform.GetChild(i);
                GameObject go = tr.gameObject;

                go.SetActive(true);
                {
                    capture.filename = tr.name;
                    capture.Capture();
                }
                go.SetActive(false);
            }
        }
    }
}