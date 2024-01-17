using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected Player Player => (Player)Owner;
    // private readonly LayerMask _groundMask;
    
    public PlayerBaseState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
        // _groundMask = LayerMask.GetMask("Ground");
    }

    // private void RotateToMousePoint()
    // {
    //     var screenPos = ((Player)Owner).InputReader.screenPos;
    //     var cameraRay = CameraManager.Instance.MainCam.ScreenPointToRay(screenPos);
    //     var rayDistance = CameraManager.Instance.MainCam.farClipPlane;
    //
    //     var isHit = Physics.Raycast(cameraRay, out var hit, rayDistance, _groundMask);
    //     if (isHit)
    //     {
    //         var dir = hit.point - Player.transform.position;
    //         dir.y = 0;
    //         dir.Normalize();
    //
    //         var lookRotation = Quaternion.LookRotation(dir);
    //         
    //         Player.Rotate(lookRotation);
    //     }
    // }
}