using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;

    protected Transform _modelTrm;

    protected Rigidbody _rigidbody;
    protected Animator _animator;

    private StateController _stateController;

    public virtual void Awake()
    {
        _modelTrm = transform.Find("Model");
        _rigidbody = GetComponent<Rigidbody>();
        _animator = _modelTrm.GetComponent<Animator>();

        _stateController = new StateController(this);
    }

    public virtual void Update()
    {
        _stateController.UpdateState();
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void AddForce(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }
}