using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Globalization;
using System.Collections.Generic;

public class VisualCodeWindow : EditorWindow
{
    private Label result;
    private ScriptGraphAsset graphAsset;

    private const string Blue = "#569CD6";    // using, public, class, void, true/false
    private const string Teal = "#4EC9B0";    // nombres de tipos: UnityEngine, MonoBehaviour, Debug
    private const string Yellow = "#DCDCAA";  // métodos: Update, Log
    private const string Orange = "#CE9178";  // textos entre comillas
    private const string Green = "#B5CEA8";   // números


    [MenuItem("Window/Textual Learning/Textual Nodes")] // Creamos La ruta que tendra la ventana
    static void ShowWindow() //Creamos la ventana
    {
        VisualCodeWindow ventana = GetWindow<VisualCodeWindow>(); //le devolvemos la ventana que abre para guardarla como una variable
        ventana.titleContent = new GUIContent("C# Textual"); // le asignamos nombre a la ventana
    }

    private void CreateGUI() //aqui trabajamos sobre la ventana
    {
        ObjectField graph = new ObjectField("Graph"); // creamos un campo de interfaz que acepte un objeto dentro de la ventana
        graph.objectType = typeof(ScriptGraphAsset); // le damos decimos que la variable tiene que ser de tipo ScriptGraphAsset que es un grafro
        graph.RegisterValueChangedCallback(evt => {graphAsset = evt.newValue as ScriptGraphAsset; ReadGraph(graphAsset);});//con esto le dejamos un recado para que cuando lo cambie ejecute la accion dada
        graph.SetValueWithoutNotify(graphAsset);
        graph.style.marginTop = 7;
        graph.style.marginLeft = 3;
        graph.style.marginRight = 3;
        rootVisualElement.Add(graph); // y lo añadimos a nuestra ventana

        result = new Label();
        result.style.whiteSpace = WhiteSpace.Pre;
        result.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
        result.style.color = new StyleColor (new Color32(220, 220, 220, 255));
        result.style.marginTop = 2;
        result.style.paddingTop = 15;
        result.style.paddingBottom = 15;
        result.style.paddingLeft = 10;
        result.style.paddingRight = 10;
        result.style.borderTopLeftRadius = 6;
        result.style.borderTopRightRadius = 6;
        result.style.borderBottomLeftRadius = 6;
        result.style.borderBottomRightRadius = 6;
        result.enableRichText = true;
        rootVisualElement.Add(result);

        rootVisualElement.schedule.Execute(() => ReadGraph(graphAsset)).Every(500); // actualiza el texto cada medio segundo

    }

    private void ReadGraph(ScriptGraphAsset graphAsset) //creamos el metodo que le los nodos
    {
        string graphName = "";
        if (graphAsset == null)
        {
            result.text = string.Empty;
            result.style.display = DisplayStyle.None;
            return;
        }

        graphName = graphAsset.name; // escribimos el nombre del grafo en la consola

        string startMetodo = "";
        List<IUnit> eventList = new List<IUnit>();

        foreach (var graphs in graphAsset.graph.units) // con graphAsset.graph.units leemos los nodos
        {
            if (graphs is Start || graphs is Update)
            {
                eventList.Add(graphs);
            }
        }


        foreach (var evenName in eventList)
        {
            string conection = "";
            IUnit current = evenName;
            int safety = 0;

            while (current != null && safety < 100)
            {
                safety++;


                if (current is InvokeMember invoke)
                {
                    string value = Paint("null", Blue);

                    foreach (var data in graphAsset.graph.valueConnections)
                    {
                        if (data.destination.unit == invoke && data.source.unit is Literal literal)
                        {
                            if (literal.type == typeof(string))
                            {
                                value = Paint("\"" + literal.value + "\"", Orange);
                            }
                            else if (literal.type == typeof(bool))
                            {
                                value = Paint(Convert.ToString(literal.value, CultureInfo.InvariantCulture).ToLower(), Blue);
                            }
                            else
                            {
                                value = Paint(Convert.ToString(literal.value, CultureInfo.InvariantCulture), Green);
                            }
                        }
                    }

                    conection += "        " + Paint(invoke.member.targetType.Name, Teal) + "." + Paint(invoke.member.name, Yellow) + "(" + value + ");\n";
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
             startMetodo += "    " +
                Paint("void", Blue) +
                " " +
                Paint(evenName.GetType().Name, Yellow) +
                "()\n" +
                "    {\n" +
                conection +
                "    }\n";
            
        }


        result.text = 
            Paint("using", Blue) +
            " " +
            Paint("UnityEngine",Teal) +
            ";\n" + 
            "\n" +
            Paint("public", Blue) +
            " " +
            Paint("class", Blue) +
            " "+ 
            Paint(graphName, Teal) +
            " : " + 
            Paint("MonoBehaviour", Teal) +
            "\n" + 
            "{\n" +
            startMetodo +
            "}";
        result.style.display = DisplayStyle.Flex;
    }

    private string Paint(string text, string hex) // NUEVO
    {
        return "<color=" + hex + ">" + text + "</color>";
    }
}
