using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InputHandlers
{
    public interface IPlatformInputHandler
    {
        public float GetAxisX();
        public float GetAxisY();
        public bool GetJump();
        public bool GetMainInteraction();
        public bool GetSubInteraction();
        public bool GetOpenInventory();

        public int GetQuickSlot();
    }
}

