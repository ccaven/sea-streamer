using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassDoorController : MonoBehaviour {

    public float targetYPosition;

    private bool activated;

    public void Activate() {

        activated = true;

    }

    private void Update() {

        if (activated) { 
        
            float dy = transform.position.y - targetYPosition;

            transform.position += Vector3.down * dy * Time.deltaTime; 
        
        }

    }

}