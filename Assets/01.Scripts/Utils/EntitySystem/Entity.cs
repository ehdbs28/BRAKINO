using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;

    public Transform ModelTrm { get; private set; }

    public CharacterController CharacterControllerCompo { get; private set; }
    public Animator AnimatorCompo { get; private set; }
    
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

    public void Move(Vector3 velocity)
    {
        CharacterControllerCompo.Move(velocity * Time.deltaTime);
    }

    public void StopImmediately()
    {
        CharacterControllerCompo.Move(Vector3.zero);
    }
}