using UnityEngine;

public class FaceUiToCamera : MonoBehaviour
{
    private Transform _transform;
    private Camera _camera;
    private Quaternion _rotation;

    private void Awake()
    {
        _transform = transform;
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        _transform.LookAt(_camera.transform.position);
        _transform.Rotate(0, 180, 0);
    }
}
