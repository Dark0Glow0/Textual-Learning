using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Globalization;

public class VisualCodeWindow : EditorWindow
{
    private Label result;
    private ScriptGraphAsset graphAsset;

    [MenuItem("Window/Textual Learning/Textual Nodes")] // Creamos La ruta que tendra la ventana
    static void ShowWindow() //Creamos la ventana
    {
        VisualCodeWindow ventana = GetWindow<VisualCodeWindow>(); //le devolvemos la ventana que abre para guardarla como una variable
        ventana.titleContent = new GUIContent("C# Textual"); // le asignamos nombre a la ventana
    }

    private void CreateGUI() //aqui trabajamos sobre la ventana
    {
        ObjectField grafo = new ObjectField("Grafo"); // creamos un campo de interfaz que acepte un objeto dentro de la ventana
        grafo.objectType = typeof(ScriptGraphAsset); // le damos decimos que la variable tiene que ser de tipo ScriptGraphAsset que es un grafro
        grafo.RegisterValueChangedCallback(evt => {graphAsset = evt.newValue as ScriptGraphAsset; ReadGraph(graphAsset);});//con esto le dejamos un recado para que cuando lo cambie ejecute la accion dada
        grafo.SetValueWithoutNotify(graphAsset);
        rootVisualElement.Add(grafo); // y lo añadimos a nuestra ventana

        result = new Label();
        result.style.whiteSpace = WhiteSpace.Pre;
        rootVisualElement.Add(result);

        rootVisualElement.schedule.Execute(() => ReadGraph(graphAsset)).Every(100); // actualiza el texto cada medio segundo
    }

    private void ReadGraph(ScriptGraphAsset graphAsset) //creamos el metodo que le los nodos
    {
        string graphName = "";
        if (graphAsset == null)
        {
            result.text = string.Empty;
            return;
        }

        graphName = graphAsset.name; // escribimos el nombre del grafo en la consola

        string startMetodo = "";
        string eventName = "";

        IUnit eventUnit = null;

        foreach (var graphs in graphAsset.graph.units) // con graphAsset.graph.units leemos los nodos
        {
            if (graphs is Start || graphs is Update)
            {
                eventName = graphs.GetType().Name;
                eventUnit = graphs;
            }
        }

        string conection = "";
        IUnit current = eventUnit;
        int safety = 0;

        while (current != null && safety < 100)
        {
            safety++;
            
            if (current is InvokeMember invoke)
            {
                string value = "null";

                foreach (var data in graphAsset.graph.valueConnections)
                {
                    if (data.destination.unit == invoke && data.source.unit is Literal literal)
                    {
                        if (literal.type == typeof(string))
                        {
                            value = "\"" + literal.value + "\"";
                        }
                        else
                        {
                            value = Convert.ToString(literal.value, CultureInfo.InvariantCulture).ToLower();
                        }
                    }
                }

                conection += "      " + invoke.member.targetType.Name + "." + invoke.member.name + "(" + value + ");\n";
            }

            IUnit next = null;
            foreach (var flow in graphAsset.graph.controlConnections)
            {
                if (flow.source.unit == current)
                {
                    next = flow.destination.unit;
                }
            }
            current = next;
        }

        if (eventName != "")
        {
            startMetodo = 
            "    void " + eventName + "()\n" +
            "    {\n" +
            conection +
            "    }\n";
        }

        result.text = 
            "using UnityEngine;\n" + 
            "\n" +
            "public class " + graphName + " : MonoBehaviour\n" + 
            "{\n" +
            startMetodo +
            "}";
    }
}
