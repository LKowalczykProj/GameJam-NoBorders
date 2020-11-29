using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{
    public Animator anim;
    GameObject player => GameObject.FindGameObjectWithTag("Player");
    Transform oldParent;
    bool once = false;

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("play animation");
            anim.Play("card2", 0);
            oldParent = player.transform.parent;
            player.transform.parent = anim.gameObject.transform;
            player.GetComponent<PlayerController>().canMove = false;
            once = true;
            StartCoroutine(WaitToMove());
        }
    }

    IEnumerator WaitToMove() {
        yield return new WaitForSeconds(10);
        player.transform.parent = oldParent;
        player.GetComponent<PlayerController>().canMove = true;
    }
}
