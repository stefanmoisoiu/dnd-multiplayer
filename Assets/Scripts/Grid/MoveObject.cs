using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MoveObject : MonoBehaviour
{
    public Action<MoveObject> OnTake { get; set; }

    [FoldoutGroup("Take Properties")] [SerializeField]
    private Vector3 rotationDir;
    [FoldoutGroup("Take Properties")] [SerializeField]
    private float rotationSpeed = 2;

    [FoldoutGroup("Taken Properties")] [SerializeField]
    private float floatHeight,forceToTargetSpeed;
    [FoldoutGroup("Taken Properties")] [SerializeField] [Range(0, 1)]
    private float velocityAffectPercent = 1;
    [FoldoutGroup("Place Properties")] [SerializeField]
    private float placeLerpDuration;
    [FoldoutGroup("Place Properties")] [SerializeField]
    private AnimationCurve placeLerpEase;
    
    [FoldoutGroup("Throw Properties")] [SerializeField]
    private float throwTorqueForce = 5;

    

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void TakenUpdate(Vector2 targetPosition)
    {
        Vector3 targetPos = new (targetPosition.x, floatHeight, targetPosition.y);
        Vector3 dir = targetPos - transform.position;
        _rb.AddForce((dir - _rb.velocity * velocityAffectPercent) * (forceToTargetSpeed * 100 * Time.deltaTime),ForceMode.Force);
        RotationTakenUpdate();
    }

    private void RotationTakenUpdate()
    {
        _rb.angularVelocity = Vector3.zero;
        _rb.MoveRotation(Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(rotationDir), Time.deltaTime * rotationSpeed));
    }
    public void Take()
    {
        OnTake?.Invoke(this);
        _rb.isKinematic = false;
        _rb.useGravity = false;
    }

    public void Place(Vector3 pos)
    {
        print("PLACEPLACEPLACE");
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(rotationDir);
        StartCoroutine(PlaceAtLerp(pos));
    }

    public void Throw()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddTorque(Random.insideUnitSphere * throwTorqueForce);
    }
    private IEnumerator PlaceAtLerp(Vector3 pos)
    {
        float advancement = 0;
        Vector3 startPos = transform.position;
        while (advancement < 1)
        {
            advancement += Time.deltaTime / placeLerpDuration;
            transform.position = Vector3.Lerp(startPos, pos, placeLerpEase.Evaluate(advancement));
            yield return null;
        }

        transform.position = pos;
    }
}
