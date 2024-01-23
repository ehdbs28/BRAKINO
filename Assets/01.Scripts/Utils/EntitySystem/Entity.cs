using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Entity : PoolableMono, IDamagable
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;

    public Transform ModelTrm { get; private set; }

    public CharacterController CharacterControllerCompo { get; private set; }
    public Animator AnimatorCompo { get; private set; }
    
    protected StateController StateController { get; private set; }

    private float _currentHp;
    
    public bool IsDead { get; private set; }

    public event Action OnHitEvent = null;

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
    
    public override void OnPop()
    {
        IsDead = false;
        _currentHp = _data.maxHp;
    }

    public override void OnPush()
    {
        OnHitEvent = null;
    }

    public void Move(Vector3 velocity)
    {
        CharacterControllerCompo.Move(velocity * Time.deltaTime);
    }

    public void StopImmediately()
    {
        CharacterControllerCompo.Move(Vector3.zero);
    }

    public void AnimationTrigger()
    {
        StateController.CurrentState.AnimationTrigger();
    }

    public void OnDamage(float damage)
    {
        _currentHp -= damage;
        OnHitEvent?.Invoke();
        if (_currentHp <= 0)
        {
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        IsDead = true;
    }
}