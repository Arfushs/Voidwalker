using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _checkRange;
    [SerializeField] private Transform _leftRay;
    [SerializeField] private Transform _rightRay;

    public bool IsGrounded()
    {
        return Physics2D.Raycast(_leftRay.position, Vector2.down, _checkRange, _whatIsGround) || 
               Physics2D.Raycast(_rightRay.position, Vector2.down, _checkRange, _whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rightRay.position, new Vector3(_rightRay.position.x, _rightRay.position.y - _checkRange, _rightRay.position.z));
        Gizmos.DrawLine(_leftRay.position, new Vector3(_leftRay.position.x, _leftRay.position.y - _checkRange, _leftRay.position.z));
    }
}
