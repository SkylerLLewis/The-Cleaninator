using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    PlayerController player;
    GameObject sparks, debris;
    void Awake() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        sparks = Resources.Load("Prefabs/Hammer Sparks") as GameObject;
        debris = Resources.Load("Prefabs/Rock Break") as GameObject;
    }

    void OnTriggerEnter(Collider c) {
        if (player.hammerActive && c.gameObject.tag == "Mineral") {
            GameObject sparksInstance = Instantiate(
                sparks,
                c.ClosestPointOnBounds(transform.position),
                Quaternion.Euler(-90, 0, 0));
            sparksInstance.name = sparksInstance.name.Split('(')[0];
            if (Random.Range(0,5) < 1) { // average 10 hits to kill
                GameObject debrisInstance = Instantiate(
                    debris,
                    c.transform.position,
                    Quaternion.Euler(-90, 0, 0));
                debrisInstance.name = debrisInstance.name.Split('(')[0];
                GameObject.Destroy(c.gameObject);
                player.HammerHit(destroyed:true);
            } else {
                player.HammerHit(destroyed:false);
            }
        }
    }
}
