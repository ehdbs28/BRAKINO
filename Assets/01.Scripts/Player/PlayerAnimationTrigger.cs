using System;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    public void AnimationEndTrigger()
    {
        _player.AnimationEndTrigger();
    }
}