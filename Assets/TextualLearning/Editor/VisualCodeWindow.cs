using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEditor.UIElements;

public class VisualCodeWindow : EditorWindow
{
    [MenuItem("Window/Textual Learning/Textual Nodes")]
    static void ShowWindow()
    {
        VisualCodeWindow ventana = GetWindow<VisualCodeWindow>();
        ventana.titleContent = new GUIContent("C# Textual");
    }

    private void CreateGUI()
    {
        Label label = new Label();
        label.text = "Hola Mundo";
        rootVisualElement.Add(label);

        ObjectField grafo = new ObjectField("Grafo");
        grafo.objectType = typeof(ScriptGraphAsset);
        grafo.RegisterValueChangedCallback(evt => Debug.Log(evt.newValue));
        rootVisualElement.Add(grafo);
    }
}
