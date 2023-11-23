using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputHandlers.Windows
{
    public class WindowsPlatformInputHandler : IPlatformInputHandler
    {
        public float GetAxisX() => Input.GetAxis("Horizontal");

        public float GetAxisY() => Input.GetAxis("Vertical");

        public bool GetJump() => Input.GetKeyDown(KeyCode.Space);

        public bool GetMainInteraction() => Input.GetMouseButton(0);

        public bool GetOpenInventory() => Input.GetKeyDown(KeyCode.E);

        public bool GetSubInteraction() => Input.GetMouseButtonDown(1);

        public int GetQuickSlot()
        {
            for (int i = 0; i < 7; i++)
                if (Input.GetKey(KeyCode.Alpha1 + i))
                    return i;
            return -1;
        }
    }
}

