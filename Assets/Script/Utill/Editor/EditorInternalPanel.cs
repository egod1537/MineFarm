using UnityEngine;

public class EditorInternalPanel<T>
{
    protected T script;
    public EditorInternalPanel(T script)
    {
        this.script = script;
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnInspectorGUI()
    {

    }
}
