using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Entity : PoolableMono, IDamageable
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;

    public Transform ModelTrm { get; private set; }

    public CharacterController CharacterControllerCompo { get; private set; }
    public Animator AnimatorCompo { get; private set; }
    
    protected StateController StateController { get; private set; }

    private float _currentHp;

    public float lastAttackTime;
    
    public bool IsDead { get; private set; }

    public event Action<Vector3> OnHitEvent = null;

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

    public virtual void Move(Vector3 velocity)
    {
        CharacterControllerCompo.Move(velocity * Time.deltaTime);
    }
    
    public virtual void StopImmediately()
    {
        CharacterControllerCompo.Move(Vector3.zero);
    }
    
    public void Rotate(Quaternion targetRot, float speed = -1)
    {
        ModelTrm.rotation = Quaternion.Lerp(ModelTrm.rotation, targetRot, speed < 0 ? Data.rotateSpeed : speed);
    }

    public void AnimationTrigger(string eventKey)
    {
        StateController.CurrentState.AnimationTrigger(eventKey);
    }

    public virtual void OnDamage(float damage, Vector3 attackedDir)
    {
        if (IsDead)
        {
            return;
        }
        
        _currentHp -= damage;
        OnHitEvent?.Invoke(attackedDir);
        if (_currentHp <= 0)
        {
            OnDead();
        }
    }
    
    protected virtual void OnDead()
    {
        IsDead = true;
    }
    
    bool IDamageable.IsDead()
    {
        return IsDead;
    }
}