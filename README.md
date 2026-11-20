# Textual Learning
![Image Reference](https://github.com/Dark0Glow0/Textual-Learning/blob/main/Images/Textual%20Visual%20Scripting.png)


**Add-on for Unity** Created to teach `C#` programming syntax, this tool is specifically designed for **_college students focusing on video games using the Unity engine_**.
> Textual Learning reads graphs created with Unity Visual Scripting and shows the C# code they represent, so learners can see how each node maps to a line of text code. Developed as part of a thesis project. Work in progress: see the roadmap for the current status.
## Requirements
```
Unity 6000.3 (6.3 LTS)
Unity Visual Scripting
```
## How to Install
1. Open a project in Unity
2. Install Unity Visual Scripting in the project
3. Download this repository (**Code > Download ZIP**) and copy the `Assets/TextualLearning` folder into your project's `Assets` folder.

## Usage

This walkthrough uses a simple "Hello World" graph. It is assumed that you have already completed the installation guide.

### 1. Create a graph
1. Create an empty GameObject in your scene.
2. Add the **Script Machine** component to it.
3. Click **New** to create a graph and save it as an asset in your project. For this example, we will name it `Test`.

### 2. Build the example
1. Open the graph with **Edit Graph**.
2. Add an **On Start** event.
3. Add a **Debug Log** node and connect On Start to it (flow connection).
4. Add a **String** node with the text `Hola Mundo` and connect it to the `Message` input (data connection).
5. Press **Play**: "Hola Mundo" should appear in the Console.

![Example graph](Images/example-graph.png)

### 3. Open Textual Nodes
Go to **Window > Textual Learning > Textual Nodes**. A window titled "C# Textual" opens.

### 4. Load your graph
Drag your graph asset from the Project window into the **Graph** field.

### 5. Read the generated code
The window shows the C# equivalent of your graph, in the same order the nodes run:

```C#
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hola Mundo");
    }
}
```

The code refreshes automatically (about every half second), so you can keep editing the graph and watch the text change.

Another example: an **On Update** event followed by two chained **Debug Log** nodes, one with an Integer `0` and one with a Boolean (unchecked), produces:

```csharp
using UnityEngine;

public class Test : MonoBehaviour
{
    void Update()
    {
        Debug.Log(0);
        Debug.Log(false);
    }
}
```

> **Note:** This tool is read-only. It does not modify your graph or run any code.

## Supported nodes

| Node | Generated C# |
|---|---|
| On Start | `void Start()` |
| On Update | `void Update()` |
| Debug Log (member call) | `Debug.Log(value);` |
| Literal (String, Integer, Float, Boolean) | `"text"`, `5`, `1.5`, `true` |

A Debug Log with no value connected is shown as `Debug.Log(null);`. Only `Debug.Log` has been tested as a member call.

## Limitations

- **Read-only.** Textual Learning displays your graph as code; it does not edit the graph, and it does not run or compile anything.
- **Limited node support.** Only the nodes listed in *Supported nodes* are translated. Unsupported nodes are currently ignored.
- **One event per graph.** All generated lines go inside a single event method.
- **Values come from Literal nodes only.** If a value comes from any other kind of node, `null` is shown.
- **Floats are written without the `f` suffix** (for example, `1.5` instead of `1.5f`).
- **Script Graphs only.** State Graphs are not supported for now.
- **Educational focus (for now).** Textual Learning is currently developed as an educational tool, as part of a thesis project. The generated C# is meant to illustrate what a graph does and may not match line by line what Unity executes internally. Broader use, including production workflows, is a possible future direction.
- **Requires Unity Visual Scripting.** The add-on reads graphs created with that package and does not work without it.
- **Tested on Unity 6000.3 (6.3 LTS).** Other versions have not been verified.

## Roadmap

**Done**
- [x] Editor window under `Window > Textual Learning > Textual Nodes`
- [x] Graph field that only accepts Script Graph assets
- [x] The window detects when a graph is assigned or removed
- [x] Read the nodes and connections inside a graph
- [x] Translate the "Hello World" graph (On Start → Debug Log → String) to C#
- [x] Follow the flow connections so lines come out in execution order
- [x] Typed values (String, Integer, Float, Boolean) and `null` for empty inputs
- [x] On Start and On Update events
- [x] Automatic refresh while the graph is edited
- [x] Dark code panel with syntax colors

**In progress**
- [ ] Code cleanup and comments

**Next**
- [ ] Automatic node translation: one translation rule per node type, so supporting a new node does not require rewriting the generator
- [ ] Short explanation of each node next to its code
- [ ] More than one event per graph
- [ ] Support for variables, math operations, `if` and loops
- [ ] Flag unsupported nodes instead of ignoring them
- [ ] Highlight the code line that corresponds to a selected node, and vice versa
- [ ] Monospace font and scrolling in the code panel
- [ ] Export the generated code as a `.cs` file
- [ ] Spanish translation of this README

**Thesis phase**
- [ ] Research and testing
- [ ] Adjustments based on the results

Ideas and suggestions are welcome: open an [issue](https://github.com/Dark0Glow0/Textual-Learning/issues).

## Credits and License

**Author:** Dark0Glow0 (Benjamin Bravo)

**Built with:**
- [Unity](https://unity.com/) by Unity Technologies
- [Unity Visual Scripting](https://docs.unity3d.com/Packages/com.unity.visualscripting@latest) (requirement, not included in this repository)

**License:** see the [LICENSE](https://github.com/Dark0Glow0/Textual-Learning/blob/main/LICENSE) file.
