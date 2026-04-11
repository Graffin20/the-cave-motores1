using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Camera Recoil")]
    public Transform cameraTarget;         // The same CameraTarget your Cinemachine camera follows
    public float cameraRecoilAmount = 2f;  // Degrees kicked up per shot
    public float cameraRiseSpeed = 20f;    // How fast the kick animates up
    public float cameraRecoverSpeed = 5f;  // How fast it returns to rest

    [Header("Viewmodel Recoil")]
    public Transform viewmodel;            // Root of your viewmodel GameObject
    public float viewmodelKickAmount = 0.1f;
    public float viewmodelRiseSpeed = 20f;
    public float viewmodelRecoverSpeed = 8f;

    private float _cameraRecoilTarget = 0f;
    private float _cameraCurrentRecoil = 0f;

    private Vector3 _viewmodelRestPos;
    private Vector3 _viewmodelTargetPos;
    private Vector3 _viewmodelCurrentPos;

    void Awake()
    {
        _viewmodelRestPos = viewmodel.localPosition;
        _viewmodelTargetPos = _viewmodelRestPos;
        _viewmodelCurrentPos = _viewmodelRestPos;
    }

    public void ApplyRecoil()
    {
        // Add to the camera recoil target (accumulates on rapid fire)
        _cameraRecoilTarget += cameraRecoilAmount;

        // Kick the viewmodel back
        _viewmodelTargetPos = _viewmodelRestPos - Vector3.forward * viewmodelKickAmount;
    }

    void Update()
    {
        HandleCameraRecoil();
        HandleViewmodelRecoil();
    }

    void HandleCameraRecoil()
    {
        // Animate current recoil toward the target (kick up)
        _cameraCurrentRecoil = Mathf.Lerp(_cameraCurrentRecoil, _cameraRecoilTarget, Time.deltaTime * cameraRiseSpeed);

        // Recover target back toward zero
        _cameraRecoilTarget = Mathf.Lerp(_cameraRecoilTarget, 0f, Time.deltaTime * cameraRecoverSpeed);

        // Apply to camera target pitch
        Vector3 euler = cameraTarget.localEulerAngles;
        float basePitch = euler.x > 180f ? euler.x - 360f : euler.x; // unwrap
        cameraTarget.localEulerAngles = new Vector3(basePitch - _cameraCurrentRecoil, euler.y, euler.z);
    }

    void HandleViewmodelRecoil()
    {
        // Animate toward kick position
        _viewmodelCurrentPos = Vector3.Lerp(_viewmodelCurrentPos, _viewmodelTargetPos, Time.deltaTime * viewmodelRiseSpeed);

        // Recover target back to rest
        _viewmodelTargetPos = Vector3.Lerp(_viewmodelTargetPos, _viewmodelRestPos, Time.deltaTime * viewmodelRecoverSpeed);

        viewmodel.localPosition = _viewmodelCurrentPos;
    }
}