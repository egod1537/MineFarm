using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minefarm.Entity.DropItem
{
    public class DropItemView : MonoBehaviour
    {
        private DropItemModel _model;
        protected DropItemModel model { get => _model = GetComponent<DropItemModel>(); }

        public TextMeshPro txtCount;

        private void Start()
        {
            txtCount.text = model.item.count.ToString();
            if (model.item.count == 1) txtCount.text = "";

            CreateModel(model.item.itemID);
        }

        private void CreateModel(ItemID itemID)
        {
            GameObject go = Instantiate(ItemDB.GetItemModel(itemID));
            Transform tr = go.transform;

            tr.SetParent(transform);
            tr.localPosition = Vector3.zero;
        }
    }
}

