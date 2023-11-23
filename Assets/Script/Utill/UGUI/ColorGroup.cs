using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGroup : MonoBehaviour
{
    [SerializeField]
    private Color _color;
    public Color color
    {
        get => _color;
        set => ChangeColor(value);
    }

    public void ChangeColor(Color color)
    {
        _color = color;

        foreach (var img in GetComponentsInChildren<Image>())
            img.color = color;
    }
}
