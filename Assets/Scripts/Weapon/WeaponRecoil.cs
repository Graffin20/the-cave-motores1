using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{


    [Header("Viewmodel Recoil")]
    public Transform viewmodel;            // Root of your viewmodel GameObject
    public float viewmodelKickAmount = 0.1f;
    public float viewmodelRiseSpeed = 20f;
    public float viewmodelRecoverSpeed = 8f;

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

        // Kick the viewmodel back
        _viewmodelTargetPos = _viewmodelRestPos - Vector3.forward * viewmodelKickAmount;
    }

    void Update()
    {
        HandleViewmodelRecoil();
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