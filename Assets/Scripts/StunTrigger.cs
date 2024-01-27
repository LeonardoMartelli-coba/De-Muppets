using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrigger : MonoBehaviour
{
    [SerializeField] private Duck duck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            duck.Stun();
        }
    }
}
