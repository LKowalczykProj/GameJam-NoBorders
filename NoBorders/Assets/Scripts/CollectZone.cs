using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            if (Input.GetKeyDown(KeyCode.E)) {
                // Trigger some animation
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		    }
        }
    }
}
