using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public bool IsActive = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && IsActive)
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.Death();
        }
    }
}
