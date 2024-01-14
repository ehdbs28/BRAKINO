using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;
    
    public Transform ModelTrm { get; private set; }

    public Rigidbody RigidbodyCompo { get; private set; }
    public Animator AnimatorCompo { get; private set; }

    public virtual void Awake()
    {
        ModelTrm = transform.Find("Model");
        RigidbodyCompo = GetComponent<Rigidbody>();
        AnimatorCompo = ModelTrm.GetComponent<Animator>();
    }

    public void SetVelocity(Vector3 velocity)
    {
        RigidbodyCompo.velocity = velocity;
    }

    public void AddForce(Vector3 force)
    {
        RigidbodyCompo.AddForce(force);
    }
}