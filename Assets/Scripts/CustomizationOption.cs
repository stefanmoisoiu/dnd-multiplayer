using UnityEngine;

[System.Serializable]
public class CustomizationOption<T>
{
    [SerializeField] private T value;
    public T Get() => value;
}