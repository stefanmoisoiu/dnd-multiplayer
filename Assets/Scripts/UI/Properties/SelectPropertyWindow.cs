using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SelectPropertyWindow : MonoBehaviour
{
    [SerializeField] internal GameObject optionGameObject;
    
    [FoldoutGroup("Layout")] [SerializeField]
    internal Transform layoutParent;

    internal void Reset()
    {
        for (int i = 0; i < layoutParent.transform.childCount; i++) Destroy(layoutParent.transform.GetChild(0).gameObject);
    }
}
