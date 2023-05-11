using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPropertyWindow : SelectPropertyWindow
{
        public delegate void ColorCallback(Color selectedColor);

        [Button("Setup Test")]
        private void SetupTest()
        {
                Setup(new []
                {
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green,
                        Color.red,Color.blue, Color.green
                },delegate(Color color) { print(color);  });
        }
        
        public void Setup(Color[] colorOptions,ColorCallback callback)
        {
                base.Reset();
                foreach (var colorOption in colorOptions)
                {
                        GameObject optionInstance = Instantiate(optionGameObject, layoutParent);
                        optionInstance.transform.GetChild(0).GetComponent<Image>().color = colorOption;
                        optionInstance.GetComponent<Button>().onClick.AddListener(delegate { callback?.Invoke(colorOption); });
                }
        }
}