using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPropertyWindow : SelectPropertyWindow
{
        private RectTransform _rectTransform;
        public delegate void ColorCallback(Color selectedColor);

        private void Start()
        {
                _rectTransform = GetComponent<RectTransform>();
        }

        public void Setup(Color[] colorOptions,ColorCallback callback)
        {
                _rectTransform.anchoredPosition = new Vector2(_rectTransform.rect.size.x, -_rectTransform.rect.size.y) / 2;
                base.Reset();
                foreach (var colorOption in colorOptions)
                {
                        GameObject optionInstance = Instantiate(optionGameObject, layoutParent);
                        optionInstance.transform.GetChild(0).GetComponent<Image>().color = colorOption;
                        optionInstance.GetComponent<Button>().onClick.AddListener(delegate { callback?.Invoke(colorOption); });
                }
        }
}