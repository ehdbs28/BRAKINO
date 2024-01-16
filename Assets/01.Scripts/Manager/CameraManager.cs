using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public Camera MainCam { get; private set; }
    
    public override void Init()
    {
        base.Init();
        MainCam = Camera.main;
    }
}