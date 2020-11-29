using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnZone : MonoBehaviour
{
    public GameObject toEnable;
    public GameObject nextCollectable;

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            if (Input.GetKeyDown(KeyCode.E)) {
                // Trigger some animation of returning and enable way to next collectable
                toEnable.SetActive(true);
                nextCollectable.SetActive(true);
		    }
        }
    }
}
