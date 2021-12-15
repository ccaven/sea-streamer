using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPadController : MonoBehaviour {

    public Transform boostDirection;

    public float boostForce;

    private bool isBoostingPlayer;

    private void OnTriggerEnter(Collider other) {
        isBoostingPlayer = true;
        
    }

    private void OnTriggerExit(Collider other) {
        isBoostingPlayer = false;
    }

    private void Update() {
        if (isBoostingPlayer) {
            PlayerMovement.instance.gameObject.GetComponent<Rigidbody>().AddForce(boostDirection.forward * boostForce, ForceMode.Impulse);
        }
    }

}