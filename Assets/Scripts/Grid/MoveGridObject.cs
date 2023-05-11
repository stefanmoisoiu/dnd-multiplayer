using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class MoveGridObject : MonoBehaviour,IGridObject
{
    [FoldoutGroup("Take Properties")] [SerializeField]
    private float lookUpLerpDuration;

    [FoldoutGroup("Take Properties")] [SerializeField]
    private Vector3 upRotation = new(-90,0,0);
    [FoldoutGroup("Take Properties")] [SerializeField]
    private AnimationCurve lookUpLerpEase;
    
    [FoldoutGroup("Taken Properties")] [SerializeField]
    private float floatHeight,lerpToTargetSpeed;
    [FoldoutGroup("Place Properties")] [SerializeField]
    private float placeLerpDuration;
    [FoldoutGroup("Place Properties")] [SerializeField]
    private AnimationCurve placeLerpEase;
    
    [FoldoutGroup("Throw Properties")] [SerializeField]
    private float throwTorqueForce = 5;

    

    private Rigidbody _rb;

    private void Start()
    {
        if (TryGetComponent(out Rigidbody rb)) _rb = rb;
    }


    public Action<IGridObject> OnTake { get; set; }
    public void TakenUpdate(Vector2 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, new(targetPosition.x, floatHeight, targetPosition.y),
            lerpToTargetSpeed * Time.deltaTime);
    }

    public void Take()
    {
        OnTake?.Invoke(this);
        StartCoroutine(LookUpLerp());
        if (_rb)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }
    }

    public void Place(Vector3 pos)
    {
        StartCoroutine(PlaceAtLerp(pos));
    }

    public void Throw()
    {
        if (!_rb) return;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.AddTorque(Random.insideUnitSphere * throwTorqueForce);
    }

    private IEnumerator LookUpLerp()
    {
        float advancement = 0;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(upRotation);
        while (advancement < 1)
        {
            advancement += Time.deltaTime / lookUpLerpDuration;
            transform.rotation = Quaternion.Lerp(startRot, targetRot, lookUpLerpEase.Evaluate(advancement));
            yield return null;
        }

        transform.rotation = targetRot;
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
