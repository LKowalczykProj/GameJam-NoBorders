using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCollider : MonoBehaviour
{
    public PolygonCollider2D col;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            col.enabled = true;
        }
    } 
}
