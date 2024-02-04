using System;
using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    private Entity _entity;

    private void Awake()
    {
        _entity = transform.parent.GetComponent<Entity>();
    }

    public void AnimationTrigger(string eventKey)
    {
        _entity.AnimationTrigger(eventKey);
    }
}