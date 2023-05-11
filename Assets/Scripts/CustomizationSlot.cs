using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class CustomizationSlot<T>
{
    public CustomizationOption<T>[] options;
    public CustomizationOption<T> SelectedOption
    {
        get
        {
            if (SelectedOption == null) return options[0];
            return SelectedOption;
        }
        set => SelectedOption = value;
    }
}
