using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogBox : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/DialogBox")]
    public static void ShowExample()
    {
        DialogBox wnd = GetWindow<DialogBox>();
        wnd.titleContent = new GUIContent("DialogBox");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
