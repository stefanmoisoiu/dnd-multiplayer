using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class ColorProperty : MonoBehaviour
{
    [SerializeField] private ColorSelectPropertyWindow selectPropertyWindow;
    private ColorSelectPropertyWindow _selectPropertyWindow;
    
    
    [Button("Force Open Select Property Window")]
    public void OpenSelectPropertyWindow()
    {
        if (_selectPropertyWindow != null) CloseSelectPropertyWindow();
        _selectPropertyWindow = Instantiate(selectPropertyWindow,transform.position,Quaternion.identity,MainCanvasManager.MainCanvasRect);
        _selectPropertyWindow.Setup(new []{Color.red,Color.green,Color.blue},SelectedColor);
    }
    [Button("Force Close Select Property Window")]
    public void CloseSelectPropertyWindow()
    {
        if (_selectPropertyWindow == null) return;
    }

    private void SelectedColor(Color color)
    {
        print($"Selected Color : {color}");
        CloseSelectPropertyWindow();
    }
}
