using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime;
    [SerializeField] private bool _followVertically;
    [SerializeField] private float _endPositionX;

    private float _startPositionX;
    private Vector3 _currentVelocity;

    private void Awake()
    {
        _startPositionX = transform.position.x;
    }

    private void LateUpdate()
    {
        var currentPosition = transform.position;
        var targetPosition = new Vector3(_target.position.x, _followVertically ? _target.position.y : currentPosition.y, currentPosition.z);

        if (targetPosition.x < _startPositionX) targetPosition.x = _startPositionX;
        if (targetPosition.x > _endPositionX) targetPosition.x = _endPositionX;

        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref _currentVelocity, _smoothTime);
    }
}