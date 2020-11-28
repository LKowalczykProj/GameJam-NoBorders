using System.Collections;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    Fade fade => GameObject.Find("Fade").GetComponent<Fade>();
    GameObject player => GameObject.FindGameObjectWithTag("Player");

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            fade.FadeOut();
            StartCoroutine(GoToSpawn());
        }
    }

    
    IEnumerator GoToSpawn() {
		yield return new WaitForSeconds(1f);
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
