using InputHandlers;
using InputHandlers.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static IPlatformInputHandler handler;

    static InputHandler()
    {
        handler = new WindowsPlatformInputHandler();
    }

    public static float GetAxisX() => handler.GetAxisX();
    public static float GetAxisY() => handler.GetAxisY();
    public static bool GetJump() => handler.GetJump();
    public static bool GetMainInteraction() => handler.GetMainInteraction();
    public static bool GetSubInteraction() => handler.GetSubInteraction();
    public static bool GetOpenInventory() => handler.GetOpenInventory();
    public static int GetQuickSlot() => handler.GetQuickSlot();
}
