using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityData _data;
    public EntityData Data => _data;

    public Rigidbody RigidbodyCompo { get; private set; }
}