using UnityEngine.UIElements;

public class UIHelpers {
    static public VisualElement BuildMenuOption(string title, int index, int selectedIndex) {
        VisualElement root = new VisualElement();
        root.AddToClassList("flex-row");
        root.AddToClassList("items-center");
        root.AddToClassList("mt-5");

        VisualElement indicator = new VisualElement();
        indicator.AddToClassList("w-4");
        indicator.AddToClassList("h-4");
        indicator.AddToClassList("border-b");
        indicator.AddToClassList("border-r");
        indicator.AddToClassList("border-black");
        indicator.AddToClassList("mt-1");
        indicator.AddToClassList("bg-yellow-500");
        indicator.AddToClassList("mr-5");
        if (index != selectedIndex) indicator.AddToClassList("opacity-0");

        Label label = new Label();
        label.AddToClassList("text-shadow");
        label.AddToClassList("text-2xl");
        label.AddToClassList("text-white");
        label.text = title;

        root.Add(indicator);
        root.Add(label);

        return root;
    }
}
