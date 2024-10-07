using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _checkRange;

    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _checkRange, _whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - _checkRange, transform.position.z));
    }
}
