using Minefarm.Entity.Actor;
using Minefarm.InGame;
using Minefarm.Item;
using Minefarm.Item.Actionable;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minefarm.Inventory.UI.Information
{
    public class ViewItemInformation : InventoryUIHahaviour
    {
        const string PATH_BUTTON_ACTIONABLE = "UI/Inventory/btnAction";

        public Image imgItem;

        public TextMeshProUGUI lblMaxHp;
        public TextMeshProUGUI lblDefense;
        public TextMeshProUGUI lblAttack;
        public TextMeshProUGUI lblAttackSpeed;
        public TextMeshProUGUI lblCritical;
        public TextMeshProUGUI lblSpeed;

        public TextMeshProUGUI lblDescrpition;

        public Transform trActioanble;

        GameObject btnActionable;

        private void Awake()
        {
            btnActionable = Resources.Load(PATH_BUTTON_ACTIONABLE) as GameObject;

            presenter.onUpdateInformation.AddListener(item => DisplayInformation(item));
        }
        public void DisplayInformation(ItemModel item)
        {
            DisplayItemStat(item);
            DisplayActionableButtons(item);
        }

        private void DisplayItemStat(ItemModel item)
        {
            imgItem.sprite = item == null ? null : ItemDB.GetItemIcon(item.itemID);

            Stats stat = item == null ? new Stats() : item.stat;
            lblMaxHp.text = $"+{stat.maxHp.ToString("#,##0")}";
            lblDefense.text = $"+{stat.defense.ToString("#,##0")} " +
                $"(+{stat.durabilityNegation.ToString("0.00")}%)";
            lblAttack.text = $"+{stat.attack.ToString("#,##0")}";
            lblAttackSpeed.text = $"+{stat.attackSpeed.ToString("0.00")}/s";
            lblCritical.text = $"+{stat.criticalChance.ToString("0.00")}% (+{(int)(stat.criticalDamage * 100f)}%)";
            lblSpeed.text = $"+{stat.speed.ToString("0.00")}/s";
        }

        private void DisplayActionableButtons(ItemModel item)
        {
            trActioanble.DestroyChild();
            if (item == null) return;

            GridLayoutGroup grid = trActioanble.GetComponent<GridLayoutGroup>();
            RectTransform rectTransform = trActioanble.GetComponent<RectTransform>();

            List<IItemActionable> actions = item.controller.actionable;
            int sz = actions.Count;

            float width = rectTransform.rect.width;
            float sizeCell = width / sz;
            grid.cellSize = new Vector2(sizeCell, grid.cellSize.y);

            for(int i=0; i < sz; i++)
            {
                GameObject go = Instantiate(btnActionable);

                Transform tr = go.transform;
                tr.SetParent(trActioanble);

                Button btn = go.GetComponent<Button>();

                int selected = i;
                btn.onClick.AddListener(() => {
                    actions[selected].Action(nowSlotItem, GameManager.ins.player); 
                });
                actions[i].DrawButton(nowSlotItem, go.GetComponentInChildren<TextMeshProUGUI>());
            }
        }
    }
}