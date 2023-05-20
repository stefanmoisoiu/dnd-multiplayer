using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TextProperty : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private TMP_Text _targetText;
    public void Setup(ref TMP_Text textToChange)
    {
        inputField.text = textToChange.text;
        _targetText = textToChange;
        inputField.onValueChanged.AddListener(delegate(string newText) { _targetText.text = newText; });
    }
    [Button("Force Open Text Effect Select Property Window")]
    public void OpenSelectPropertyWindow()
    {
        // if (_selectPropertyWindow != null) CloseSelectPropertyWindow();
        // _selectPropertyWindow = Instantiate(selectPropertyWindow,transform.position,Quaternion.identity,MainCanvasManager.MainCanvasRect);
    }
    [Button("Force Close Select Property Window")]
    public void CloseSelectPropertyWindow()
    {
        // if (_selectPropertyWindow == null) return;
    }
}
