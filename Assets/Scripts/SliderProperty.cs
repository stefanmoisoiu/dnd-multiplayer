using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SliderProperty : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text nameText;
    
    public void Setup(string text,float minValue, float maxValue,UnityAction<float> onValueChanged,float defaultValue = -1)
    {
        nameText.text = text;
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = defaultValue == -1 ? minValue : defaultValue;
        slider.onValueChanged.AddListener(onValueChanged);
    }
}
