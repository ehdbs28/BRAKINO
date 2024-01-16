using System;

public class GameManager : MonoSingleton<GameManager>
{
    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        CameraManager.Instance.Init();
        PoolManager.Instance.Init();
    }
}