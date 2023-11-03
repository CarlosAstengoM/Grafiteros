using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _zoomStep;


    [SerializeField] private float _minOffset;
    [SerializeField] private float _maxOffset;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }


    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }


    private void HandleMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;

        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * _moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        if(Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1f;
        }
        if(Input.GetKey(KeyCode.E))
        {
            rotationVector.y -=1f;
        }

        transform.eulerAngles += rotationVector * _rotateSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= _zoomStep;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += _zoomStep;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, _minOffset, _maxOffset);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
