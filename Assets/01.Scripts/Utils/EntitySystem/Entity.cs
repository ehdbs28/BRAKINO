using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;

    protected Transform ModelTrm { get; private set; }

    protected CharacterController CharacterControllerCompo { get; private set; }
    protected Animator AnimatorCompo { get; private set; }

    protected StateController StateController { get; private set; }

    public virtual void Awake()
    {
        ModelTrm = transform.Find("Model");
        CharacterControllerCompo = GetComponent<CharacterController>();
        AnimatorCompo = ModelTrm.GetComponent<Animator>();

        StateController = new StateController(this);
    }

    public virtual void Update()
    {
        StateController.UpdateState();
    }

    public void SetVelocity(Vector3 velocity)
    {
        CharacterControllerCompo.Move(velocity * Time.deltaTime);
    }

    public void StopImmediately()
    {
        CharacterControllerCompo.Move(Vector3.zero);
    }
}