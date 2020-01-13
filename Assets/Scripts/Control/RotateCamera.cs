using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] bool LookAtPlayer = false;
    [SerializeField] bool RotateAroundPlayer = true;
    [SerializeField] float RotationSpeed = 5.0f;

    Vector3 _cameraOffset;
    [Range(0.01f, 1.0f)] public float SmoothFactor = 0.5f;

    private void Start()
    {
        _cameraOffset = transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        if (RotateAroundPlayer)
        {
            if (Input.GetMouseButton(1))
            {
                Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
                _cameraOffset = camTurnAngle * _cameraOffset;
            }
        }



        Vector3 newPos = playerTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer || RotateAroundPlayer)
            transform.LookAt(playerTransform);
    }
}