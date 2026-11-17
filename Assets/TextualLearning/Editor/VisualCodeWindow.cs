using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEditor.UIElements;

public class VisualCodeWindow : EditorWindow
{
    private Label result;

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
        grafo.RegisterValueChangedCallback(evt => ReadGraph(evt.newValue as ScriptGraphAsset)); //con esto le dejamos un recado para que cuando lo cambie ejecute la accion dada
        rootVisualElement.Add(grafo); // y lo añadimos a nuestra ventana

        result = new Label();
        result.style.whiteSpace = WhiteSpace.Pre;
        rootVisualElement.Add(result);
    }

    private void ReadGraph(ScriptGraphAsset graphAsset) //creamos el metodo que le los nodos
    {
        string graphName = "";
        if (graphAsset == null)
        {
            return;
        }

        graphName = graphAsset.name; // escribimos el nombre del grafo en la consola

        string clase = "";
        string metodo = "";
        string valor = "";

        foreach (var graphs in graphAsset.graph.units) // con graphAsset.graph.units leemos los nodos
        {
            Debug.Log(graphs.GetType().Name); // aqui nos da el resultado con el nombre de sus tipos 

            if (graphs is InvokeMember invokeMember) // con este if consultamos a que miembro de InvokeMember pertenece el nodo y lo escribimos en consola
            {
                metodo = invokeMember.member.name; //dice cual es el metodo
                clase = invokeMember.member.targetType.Name; // dice a que clase pertenece el metodo
            }

            if (graphs is Literal literal) // con este if consultamos el valor que tiene el nodo Literal y se lo damos en consola
            {
                valor = literal.value.ToString();
            }
        }

        //result.text = clase +"." +metodo + "(\"" + valor + "\");";
        result.text = 
            "using UnityEngine;\n" + 
            "\n" +
            "public class " + graphName + " : MonoBehaviour\n" + 
            "{\n" +
            "    void Start()\n" + 
            "    {\n" + 
            "      " + clase + "." + metodo + "(\"" + valor + "\");\n" + 
            "    }\n" +
            "}";

        foreach (var connctions in graphAsset.graph.controlConnections) // recorremos la concxion entre start a InvokeMember que seria el flujo
        {
            Debug.Log(connctions.source.unit.GetType().Name + " -> " + connctions.destination.unit.GetType().Name);
        }

        foreach (var connctions in graphAsset.graph.valueConnections) // recorremos la conecxion entre Liteal a InvokeMember que seria el dato
        {
            Debug.Log(connctions.source.unit.GetType().Name + " -> " + connctions.destination.unit.GetType().Name);
        }
      
    }
}
