using System.Collections;
using UnityEngine;

public class PoolableParticle : PoolableMono
{
    private ParticleSystem _particleSystem;
    private Coroutine _runningRoutine;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetPositionAndRotation(Vector3 pos = default, Quaternion rot = default)
    {
        _particleSystem.transform.SetPositionAndRotation(pos, rot);
    }

    public void Play()
    {
        if (_runningRoutine is not null)
        {
            StopCoroutine(_runningRoutine);
        }
        _runningRoutine = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        _particleSystem.Play();
        yield return new WaitUntil(() => _particleSystem.isStopped);
        PoolManager.Instance.Push(this);
    }

    public override void OnPop()
    {
        _runningRoutine = null;
    }

    public override void OnPush()
    {
        _particleSystem.Stop();
    }
}