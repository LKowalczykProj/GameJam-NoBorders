using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalTrigger : MonoBehaviour
{
    public PlayableDirector director;
    public Rigidbody2D rb;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Trigger some animation
                rb.isKinematic = true;
                director.Play();

            }
        }
    }
}
